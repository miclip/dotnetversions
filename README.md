# Dotnetversion 

Simple application that displays installed versions of .NET based on the registry. Useful for containers where remote access is difficult. 

Based on this article: https://msdn.microsoft.com/en-us/library/hh925568%28v=vs.110%29.aspx?f=255&MSPPError=-2147217396

Deploy to PCF: 

cd to dotnetversions directory that contains csproj 

`dotnet restore `

`dotnet build -f net452`

`dotnet publish `

`cf push -p bin\Release\PublishOutput`

Some of the versions will be repeated due to how MSFT changes how they registered .NET versions. Typcially you'll be looking for the latest. 
