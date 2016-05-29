##OwinClientCert##

	1.  Generate root certificate

        makecert.exe -r -n "CN=RootCertificate" -pe -sv c:\temp\RootCertificate.pvk -a sha1 -len 2048 -b 01/01/2015 -e 01/01/2030 -cy authority c:\temp\RootCertificate.cer
	
	2.  Package the .pvk and .cer files into a .pfx file using the pvk2pfx.exe tool 
	
	    pvk2pfx.exe -pvk c:\temp\RootCertificate.pvk -spc c:\temp\RootCertificate.cer -pfx c:\temp\RootCertificate.pfx -pi <password>

    3.  Install root certificate into Trusted root folder on LocalMachine storage
	
	4.  Create server certificate

	    makecert.exe -ic c:\temp\RootCertificate.cer -iv c:\temp\RootCertificate.pvk -pe -sv c:\temp\localtestservercert.pvk -a sha1 -n "CN=mydomain.com" -len 2048 -b 01/01/2015 -e 01/01/2030 -sky exchange c:\temp\localtestservercert.cer -eku 1.3.6.1.5.5.7.3.1
	    pvk2pfx.exe -pvk c:\temp\localtestservercert.pvk -spc c:\temp\localtestservercert.cer -pfx c:\temp\localtestservercert.pfx -pi <password>

	5.  Install server certificate into Personal folder on LocalMachine storage
	
	6.  Register ssl
	
	    netsh http add sslcert ipport=0.0.0.0:8099 appid={214124cd-d05b-4309-9af9-9caa44b2b74a} certhash=<thumbprint>
	
		Verify bindings:
	    netsh http show sslcert

	7. Create client certificate

	    makecert.exe -ic c:\temp\RootCertificate.cer -iv c:\temp\RootCertificate.pvk -pe -sv c:\temp\localtestclientcert.pvk -a sha1 -n "CN=MyClientCert" -len 2048 -b 01/01/2015 -e 01/01/2030 -sky exchange c:\temp\localtestclientcert.cer -eku 1.3.6.1.5.5.7.3.2
	
	    pvk2pfx.exe -pvk c:\temp\localtestclientcert.pvk -spc c:\temp\localtestclientcert.cer -pfx c:\temp\localtestclientcert.pfx -pi <password>

	8.  Install client certificate into Personal folder on LocalMachine storage
	         
