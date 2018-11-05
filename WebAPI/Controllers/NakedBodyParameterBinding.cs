using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.ModelBinding;
using System.Web.Http.Metadata;

namespace WebAPI
{
    /// <summary>
    /// Reads the Request body into a string/byte[] and
    /// assigns it to the parameter bound.
    /// 
    /// Should only be used with a single parameter on
    /// a Web API method using the [NakedBody] attribute
    /// https://weblog.west-wind.com/posts/2013/Dec/13/Accepting-Raw-Request-Body-Content-with-ASPNET-Web-API
    /// </summary>
    public class NakedBodyParameterBinding : HttpParameterBinding
    {
        /// <summary/>
        public NakedBodyParameterBinding(HttpParameterDescriptor descriptor)
            : base(descriptor)
        {

        }

        /// <summary/>
        public override Task ExecuteBindingAsync(System.Web.Http.Metadata.ModelMetadataProvider metadataProvider,
                                                 HttpActionContext actionContext, 
                                                 CancellationToken cancellationToken)
        {
        //    throw new NotImplementedException();
        //}

        //public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider,
        //                                            HttpActionContext actionContext,
        //                                            CancellationToken cancellationToken)
        //{
            var binding = actionContext
                .ActionDescriptor
                .ActionBinding;

            if (binding.ParameterBindings.Length > 1 ||
                actionContext.Request.Method == HttpMethod.Get)
                return EmptyTask.Start();

            var type = binding
                        .ParameterBindings[0]
                        .Descriptor.ParameterType;

            if (type == typeof(string))
            {
                return actionContext.Request.Content
                        .ReadAsStringAsync()
                        .ContinueWith((task) =>
                        {
                            var stringResult = task.Result;
                            SetValue(actionContext, stringResult);
                        });
            }
            else if (type == typeof(byte[]))
            {
                return actionContext.Request.Content
                    .ReadAsByteArrayAsync()
                    .ContinueWith((task) =>
                    {
                        byte[] result = task.Result;
                        SetValue(actionContext, result);
                    });
            }

            throw new InvalidOperationException("Only string and byte[] are supported for [NakedBody] parameters");
        }

        /// <summary/>
        public override bool WillReadBody
        {
            get
            {
                return true;
            }
        }
    }

    /// <summary>
    /// A do nothing task that can be returned
    /// from functions that require task results
    /// when there's nothing to do.
    /// 
    /// This essentially returns a completed task
    /// with an empty value structure result.
    /// </summary>
    public class EmptyTask
    {
        /// <summary/>
        public static Task Start()
        {
            var taskSource = new TaskCompletionSource<AsyncVoid>();
            taskSource.SetResult(default(AsyncVoid));
            return taskSource.Task as Task;
        }

        private struct AsyncVoid
        {
        }
    }

    /// <summary>
    /// An attribute that captures the entire content body and stores it
    /// into the parameter of type string or byte[].
    /// </summary>
    /// <remarks>
    /// The parameter marked up with this attribute should be the only parameter as it reads the
    /// entire request body and assigns it to that parameter.    
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public sealed class NakedBodyAttribute : System.Web.Http.ParameterBindingAttribute
    {
        /// <summary/>
        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            if (parameter == null)
                throw new ArgumentException("Invalid parameter");

            return new NakedBodyParameterBinding(parameter);
        }
    }

}