@echo off
SET APPSETTING_ScmType=GitHub
SET APPSETTING_WEBSITE_AUTH_ENABLED=False
SET APPSETTING_WEBSITE_NODE_DEFAULT_VERSION=0.10.32
SET APPSETTING_WEBSITE_SITE_NAME=cakekudu
SET branch=master
SET command=deploy.cmd
SET deployment_branch=master
SET DEPLOYMENT_SOURCE=%CD%
SET DEPLOYMENT_TARGET=%CD%\LocalDeploy\wwwroot
SET DEPLOYMENT_TEMP=%CD%\LocalDeploy\Temp
SET KUDU_SYNC_CMD=kudusync
SET MSBUILD_PATH=D:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe
SET ___NEXT_MANIFEST_PATH=D:\home\site\deployments\cb184fa03bcde2d39e70ca661f75435dfa8f8311\manifest
SET NPM_JS_PATH=D:\Program Files (x86)\npm\1.4.28\node_modules\npm\bin\npm-cli.js
SET NUGET_EXE=C:\Program Files (x86)\SiteExtensions\Kudu\47.40908.1797\bin\scripts\nuget.exe
SET ___PREVIOUS_MANIFEST_PATH=D:\home\site\deployments\53f52a08d83b0053b7eba4ed241889ca4eda09c2\manifest
SET REGION_NAME=North Europe
SET SCM_BUILD_ARGS=
SET SCM_COMMAND_IDLE_TIMEOUT=60
SET SCM_COMMIT_ID=cb184fa03bcde2d39e70ca661f75435dfa8f8311
SET SCM_DNVM_PS_PATH=C:\Program Files (x86)\SiteExtensions\Kudu\47.40908.1797\bin\scripts\dnvm.ps1
SET SCM_GIT_EMAIL=windowsazure
SET SCM_GIT_USERNAME=windowsazure
SET SCM_LOGSTREAM_TIMEOUT=1800
SET SCM_TRACE_LEVEL=1
SET ScmType=GitHub
SET WEBROOT_PATH=%CD%\LocalDeploy\wwwroot
SET WEBSITE_AUTH_ENABLED=False
SET WEBSITE_COMPUTE_MODE=Shared
SET WEBSITE_HOSTNAME=cakekudu.azurewebsites.net
SET WEBSITE_HTTPLOGGING_ENABLED=0
SET WEBSITE_IIS_SITE_NAME=~1cakekudu
SET WEBSITE_INSTANCE_ID=0991c182e75e27ae1ae75c5079fd8847da8f5bb675f505a24abf2ff6b87c8c89
SET WEBSITE_NODE_DEFAULT_VERSION=0.10.32
SET WEBSITE_OWNER_NAME=01bf0ebb-8e2f-4759-b298-e27cf768979c+devlead-NorthEuropewebspace
SET WEBSITE_SCM_ALWAYS_ON_ENABLED=0
SET WEBSITE_SCM_SEPARATE_STATUS=1
SET WEBSITE_SITE_MODE=Limited
SET WEBSITE_SITE_NAME=cakekudu
SET WEBSITE_SKU=Free
SET WEBSOCKET_CONCURRENT_REQUEST_LIMIT=5
IF NOT EXIST "%cd%\LocalDeploy" (md "%cd%\LocalDeploy")
IF NOT EXIST "%cd%\LocalDeploy\Temp" (md "%cd%\LocalDeploy\Temp")
IF NOT EXIST "%cd%\LocalDeploy\wwwroot" (md "%cd%\LocalDeploy\wwwroot")
CALL deploy.cmd