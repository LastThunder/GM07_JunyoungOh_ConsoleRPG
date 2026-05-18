# 콘솔 RPG

## 개요
MSA 구조를 로컬로 연습해 보기 위한 콘솔RPG
<br/>

## 주요내용
<img width="230" height="500" alt="260517_GM_07_오준영_과제18 (7)" src="https://github.com/user-attachments/assets/6a9fdca7-204b-43fa-a413-1308e5eb49ee" align="left" hspace="20">

1. 각 DB server
 - 유저&캐릭터 / 아이템 / 상점, 결제 / 게임 정보 등을 저장  (DB는 로컬 파일로 대체)
 - 서버 요청 시, 해당 데이터 제공

2. 각 App Server
 - 주요 로직 별 데이터 처리 후 해당 DB 서버에 저장 요청 / 클라에 반환<br/>
   (DB 부담을 줄이기 위해 비가역적 데이터는 csv/json 등을 이용)

3. API GateWay (서버)
 - 클라/App Server가 요청한 처리를 담당 App 서버로 분산 / 결과 통합 후 클라에 리턴
 - 인증 / 로깅도 주로 여기서 진행

4. 클라
 - 화면 입출력 제공
 - 인게임 로직 처리 (게임 서버를 이용할 때도 있음)
 - 필요한 정보 or 처리를 서버에 요청 (API 호출)<br/>
   (서버 부담을 줄이기 위해 비가역적 데이터는 csv/json 등을 이용)

<br clear="left">
