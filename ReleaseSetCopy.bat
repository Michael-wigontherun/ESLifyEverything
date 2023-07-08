echo Copying ESLify Everything Release Build to ESLify Everything Release Set
xcopy /s /y ".\ESLifyEverything\bin\Release\net6.0" ".\ReleaseSets\ESLifyEverything\ESLifyEverything"
echo Copying Champollion and BSA Browser to ESLify Everything Release Set
xcopy /s /y ".\OtherAddins" ".\ReleaseSets\ESLifyEverything\ESLifyEverything"

echo Copying MergifyBashTags Release Build to Release Set
xcopy /s /y ".\MergifyBashTags\bin\Release\net6.0" ".\ReleaseSets\MergifyBashTags\MergifyBashTags"