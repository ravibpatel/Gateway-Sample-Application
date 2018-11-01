; Script generated by the HM NIS Edit Script Wizard.

; HM NIS Edit Wizard helper defines
!define PRODUCT_NAME "Gateway Sample Application"
!define PRODUCT_VERSION "1.0"
!define PRODUCT_PUBLISHER "RBSoft, Inc."
!define PRODUCT_WEB_SITE "http://rbsoft.org"
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\Gateway Sample Application.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

SetCompressor lzma

; MUI 1.67 compatible ------
!include "MUI.nsh"
!include "DotNetChecker.nsh"

; MUI Settings
!define MUI_ABORTWARNING
!define MUI_ICON "${NSISDIR}\Contrib\Graphics\Icons\nsis3-install.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\nsis3-uninstall.ico"

; Welcome page
!insertmacro MUI_PAGE_WELCOME
; License page
!insertmacro MUI_PAGE_LICENSE "..\bin\Release\License.txt"
; Directory page
!insertmacro MUI_PAGE_DIRECTORY
; Instfiles page
!insertmacro MUI_PAGE_INSTFILES
; Finish page
!define MUI_FINISHPAGE_RUN "$INSTDIR\Gateway Sample Application.exe"
!insertmacro MUI_PAGE_FINISH

; Uninstaller pages
!insertmacro MUI_UNPAGE_INSTFILES

; Language files
!insertmacro MUI_LANGUAGE "English"

; Reserve files
!insertmacro MUI_RESERVEFILE_INSTALLOPTIONS

; MUI end ------

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "..\..\Gateway.Sample.Application.Setup.exe"
InstallDir "$PROGRAMFILES\RBSoft\Gateway Sample Application"
InstallDirRegKey HKLM "${PRODUCT_DIR_REGKEY}" ""
ShowInstDetails show
ShowUnInstDetails show

Section "MainSection" SEC01
  !insertmacro CheckNetFramework 462
  
  SetOutPath "$INSTDIR"
  File "..\bin\Release\Gateway Sample Application.exe"
  CreateDirectory "$SMPROGRAMS\Gateway Sample Application"
  CreateShortCut "$SMPROGRAMS\Gateway Sample Application\Gateway Sample Application.lnk" "$INSTDIR\Gateway Sample Application.exe"
  CreateShortCut "$DESKTOP\Gateway Sample Application.lnk" "$INSTDIR\Gateway Sample Application.exe"
  File "..\bin\Release\Gateway Sample Application.exe.config"
  File "..\bin\Release\Gateway Sample Application.pdb"
  File "..\bin\Release\License.txt"
  File "..\bin\Release\Newtonsoft.Json.dll"
  File "..\bin\Release\Newtonsoft.Json.xml"
SectionEnd

Section -AdditionalIcons
  WriteIniStr "$INSTDIR\${PRODUCT_NAME}.url" "InternetShortcut" "URL" "${PRODUCT_WEB_SITE}"
  CreateShortCut "$SMPROGRAMS\Gateway Sample Application\Website.lnk" "$INSTDIR\${PRODUCT_NAME}.url"
  CreateShortCut "$SMPROGRAMS\Gateway Sample Application\Uninstall.lnk" "$INSTDIR\uninst.exe"
SectionEnd

Section -Post
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR\Gateway Sample Application.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\Gateway Sample Application.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
SectionEnd


Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) was successfully removed from your computer."
FunctionEnd

Function un.onInit
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "Are you sure you want to completely remove $(^Name) and all of its components?" IDYES +2
  Abort
FunctionEnd

Section Uninstall
  Delete "$INSTDIR\${PRODUCT_NAME}.url"
  Delete "$INSTDIR\uninst.exe"
  Delete "$INSTDIR\Newtonsoft.Json.xml"
  Delete "$INSTDIR\Newtonsoft.Json.dll"
  Delete "$INSTDIR\License.txt"
  Delete "$INSTDIR\Gateway Sample Application.pdb"
  Delete "$INSTDIR\Gateway Sample Application.exe.config"
  Delete "$INSTDIR\Gateway Sample Application.exe"

  Delete "$SMPROGRAMS\Gateway Sample Application\Uninstall.lnk"
  Delete "$SMPROGRAMS\Gateway Sample Application\Website.lnk"
  Delete "$DESKTOP\Gateway Sample Application.lnk"
  Delete "$SMPROGRAMS\Gateway Sample Application\Gateway Sample Application.lnk"

  RMDir "$SMPROGRAMS\Gateway Sample Application"
  RMDir "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"
  SetAutoClose true
SectionEnd