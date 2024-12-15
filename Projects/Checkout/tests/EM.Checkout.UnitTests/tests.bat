# Executa o projeto de testes
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=TestResults\Coverage\ 

# Gera o relatório de cobertura através do *Report Generator* em html.
reportGenerator "-reports:TestResults\Coverage\coverage.cobertura.xml" "-targetdir:TestResults\Coverage\Reports" -reporttypes:Html
.\TestResults\Coverage\Reports\index.html