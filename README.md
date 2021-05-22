# perseverance

## 00 - Solution

```cmd
PS C:\Github\perseverance> Get-History

  Id CommandLine
  -- -----------
   1 cd C:\Github\
   2 git clone https://github.com/sheltertake/perseverance.git
   3 cd .\perseverance\
   4 git checkout -b 01/solution
   5 git push
   6 git push --set-upstream origin 01/solution
   7 dotnet new gitignore
   8 dotnet new nunit -n PerseveranceUnitTests -o tests/PerseveranceUnitTests
   9 dotnet new nunit -n PerseveranceFunctionalTests -o tests/PerseveranceFunctionalTests
  11 dotnet new solution -n Perseverance
  13 dotnet new classlib -n Perseverance -o src/Perseverance

```

 - open vs
 - refine solution
 - update nuget
 - install Moq + FluentAssertion + SpecFlow.NUnit