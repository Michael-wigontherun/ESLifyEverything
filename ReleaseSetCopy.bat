echo Copying ESLify Everything Release Build to ESLify Everything Release Set
xcopy /s /y ".\ESLifyEverything\bin\Release\net6.0" ".\ReleaseSets\ESLifyEverything\ESLifyEverything"
echo Copying Champollion and BSA Browser to ESLify Everything Release Set
xcopy /s /y ".\OtherAddins" ".\ReleaseSets\ESLifyEverything\ESLifyEverything"

echo Deleting ReleaseSets\ESLifyEverything.rar
del \q ".\ReleaseSets\ESLifyEverything.rar"

echo Creating ESLifyEverything.rar
"C:\Program Files\WinRAR\WinRAR.exe" a -s -ep1 ".\ReleaseSets\ESLifyEverything.rar" ".\ReleaseSets\ESLifyEverything\ESLifyEverything" ".\ReleaseSets\ESLifyEverything\Edit Scripts"

echo Copying MergifyBashTags Release Build to Release Set
xcopy /s /y ".\MergifyBashTags\bin\Release\net6.0" ".\ReleaseSets\MergifyBashTags\MergifyBashTags"

echo Deleting ReleaseSets\MergifyBashTags.rar
del \q ".\ReleaseSets\MergifyBashTags.rar"

echo Creating MergifyBashTags.rar
"C:\Program Files\WinRAR\WinRAR.exe" a -s -ep1 ".\ReleaseSets\MergifyBashTags.rar" ".\ReleaseSets\MergifyBashTags\MergifyBashTags"

explorer.exe ".\ReleaseSets"