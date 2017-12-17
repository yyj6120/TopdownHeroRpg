* 해당 DLL은 실제로 사용되지 않는다.
	- IIS에서 호스팅을 위해서는 svc파일 web.config 파일만 필요할 뿐 해당 DLL은 필요가 없다.
	- 이 프로젝트가 존재하는 이유는 디버깅의 용이함을 위해서이다.
	- 배포시에 이 프로젝트의 DLL을 배포 하지 않아도 상관 없으나 차후 사용 될지도 모르기 때문에 같이 배포하도록 한다.

* IIS 호스팅 시 디버깅.
	- 바이너리 파일은 Build/Bin에 배포 되기 때문에 디폴트 셋팅으로 디버깅 할 수 없다.
	- IIS에서 호스팅 할 시에는 IIS에 응용 프로그램을 추가를 한다. (실제 경로는 Build 폴더로 지정한다.)
	- IIS Express 호스팅 시에는 C:\Users\사용자\Documents\IISExpress\config\applicationhost.config 파일을 열어 아래 내용을 추가 하여 준다.
			<application path="/">
                    <virtualDirectory path="/" physicalPath="%IIS_SITES_HOME%\WebSite1" />
                    <virtualDirectory path="/가상디렉토리명" physicalPath="물리적인폴더위치" />
             </application>

* IIS 셋팅시 트러블 슈팅.
	- WebDeploy를 설치하면 편하다 
		- 참고 글 : http://xyz37.blog.me/50108758419 (해당 사이트는 Web Deploy 2.0으로 설명하고 있는데 3.5로 설치하면 되며 3.5로 설치 시 관리서비스, IIS 관리자 외 부분은 설정이 자동으로 된다)

	- 404 에러시
		- WCF관련(.Net 3.5) 모듈 설치가 되지 않았을 경우에 발생
		- C:\Windows\Microsoft .NET\Framework\3.0\Windows Communication Foundation\ServiceModelReg.exe -i 실행
		- 참고 글 : http://blog.daum.net/lei1400/11

	- 500 에러시
		- C:\Windows\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe -i 실행
		- 참고 글 : http://blog.naver.com/PostView.nhn?blogId=bluelevel&logNo=70109765657

	- 503 에러시
		- IIS 응용 프로그램풀에 WCF 서비스가 등록 된 풀이 동작중인지 확인한다
		- 동작중이 아니라면 동작시켜준다

	- Visual Studio에서 실행 시 에러 날 때.
		- IIS의 응용 프로그램 풀이 .Net Framework 버전이 맞는지 확인한다.
