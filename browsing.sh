#!/bin/bash
# Is Directory Browsing allowed?
# toggle /enabled:true (or false)
/mnt/c/Program\ Files/IIS\ Express/appcmd.exe set config /section:directoryBrowse /enabled:true

echo 
echo "You may need to add or remove from your web.config:"
echo '  <system.webServer>'
echo '    <directoryBrowse enabled="true" />'
echo '    <modules runAllManagedModulesForAllRequests="true" />'
echo '  </system.webServer>'

