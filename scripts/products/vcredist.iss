[CustomMessages]
vcredist2010_title=Visual C++ 2010 SP1 Redistributable
vcredist2010_title_x64=Visual C++ 2010 64-Bit SP1 Redistributable
vcredist2013_title=Visual C++ 2013 Redistributable
vcredist2013_title_x64=Visual C++ 2013 64-Bit Redistributable

en.vcredist2010_size=4.8 MB
de.vcredist2010_size=4,8 MB
en.vcredist2013_size=6.2 MB
de.vcredist2013_size=6,2 MB

en.vcredist2010_size_x64=5.5 MB
de.vcredist2010_size_x64=5,5 MB
en.vcredist2013_size_x64=6.9 MB
de.vcredist2013_size_x64=6,9 MB

[Code]
const
	vcredist2010_url = 'http://download.microsoft.com/download/C/6/D/C6D0FD4E-9E53-4897-9B91-836EBA2AACD3/vcredist_x86.exe';
	vcredist2010_url_x64 = 'http://download.microsoft.com/download/A/8/0/A80747C3-41BD-45DF-B505-E9710D2744E0/vcredist_x64.exe';
  vcredist2013_url = 'http://download.microsoft.com/download/2/E/6/2E61CFA4-993B-4DD4-91DA-3737CD5CD6E3/vcredist_x86.exe';
	vcredist2013_url_x64 = 'http://download.microsoft.com/download/2/E/6/2E61CFA4-993B-4DD4-91DA-3737CD5CD6E3/vcredist_x64.exe';

	vcredist2010_productcode = '{F0C3E5D1-1ADE-321E-8167-68EF0DE699A5}';
	vcredist2010_productcode_x64 = '{1D8E6291-B0D5-35EC-8441-6616F567A0F7}';
  vcredist2013_productcode = '{13A4EE12-23EA-3371-91EE-EFB36DDFFF3E}';
	vcredist2013_productcode_x64 = '{A749D8E6-B613-3BE3-8F5F-045C84EBA29B}';

procedure vcredist2010();
begin
	if (not msiproduct(vcredist2010_productcode)) then
		AddProduct('vcredist2010_x86.exe',
			'/q /norestart',
			CustomMessage('vcredist2010_title'),
			CustomMessage('vcredist2010_size'),
			vcredist2010_url,
			false, false);
  if (Is64BitInstallMode) then
    if (not msiproduct(vcredist2010_productcode_x64)) then
		AddProduct('vcredist2010_x64.exe',
			'/q /norestart',
			CustomMessage('vcredist2010_title_x64'),
			CustomMessage('vcredist2010_size_x64'),
			vcredist2010_url_x64,
			false, false);	
end;

procedure vcredist2013();
begin
	if (not msiproduct(vcredist2013_productcode)) then
		AddProduct('vcredist2013_x86.exe',
			'/q /norestart',
			CustomMessage('vcredist2013_title'),
			CustomMessage('vcredist2013_size'),
			vcredist2013_url,
			false, false);
  if (Is64BitInstallMode) then
    if (not msiproduct(vcredist2013_productcode_x64)) then
		AddProduct('vcredist2013_x64.exe',
			'/q /norestart',
			CustomMessage('vcredist2013_title_x64'),
			CustomMessage('vcredist2013_size_x64'),
			vcredist2013_url_x64,
			false, false);	
end;