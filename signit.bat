@REM Adjust this batch file, but do not include the certificate or parameters in your repository

@REM @signtool.exe sign /ac <certificate chain.cer> /f <encrypted certificate.pfx> /p <certificate password> /t "http://timestamp.verisign.com/scripts/timestamp.dll" /v %*