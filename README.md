# :pushpin: ARMS 모작
메타버스 아카데미 2차 프로젝트

</br>

![image](https://github.com/MightyChipmunk/ARMS/assets/35093963/ad9ed702-bbd6-4eda-bb5a-ea5472cb1a81)
![image](https://github.com/MightyChipmunk/ARMS/assets/35093963/56461be3-fd1e-4682-a6cb-21863f039aba)

</br>

## 1. 제작 기간 & 참여 인원
- 2022년 8월 ~ 9월 (1달)
- 유니티 개발자 3명

</br>

## 2. 사용 기술
 - <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white"> 
 - <img src="https://img.shields.io/badge/Unity-FFFFFF?style=for-the-badge&logo=unity&logoColor=black"> 
 - <img src="https://img.shields.io/badge/git-F05032?style=for-the-badge&logo=git&logoColor=white">

</br>


## 3. 핵심 기능 및 상세 역할
- 1대 1 액션게임을 구현해 간단한 AI 로직 구현 및 3인칭 카메라 / Charactor Controller를 통한 플레이어 움직임 구현에 중점을 뒀습니다. 
- ARMS라는 스위치로 출시된 액션 게임을 모작하였습니다.



<details>
<summary><b>핵심 기능 설명 펼치기</b></summary>
<div markdown="1">

### 3.1. 씬 구성
![](https://velog.velcdn.com/images/kjhdx/post/9bceff69-b608-45d4-8f07-bb56f0145b64/image.png)
  
### 3.2. 무기 (주먹) 구현

- 일반 주먹
  - 일반적인 직선 궤적으로 나아갑니다.
  ![](https://velog.velcdn.com/images/kjhdx/post/913074ec-d3b1-4a33-ba9d-f5029cec8f1e/image.png)


- 레이저
  - 즉발 형식의 레이저를 쏩니다.
  ![](https://velog.velcdn.com/images/kjhdx/post/a0e8446d-8c3f-4c0a-9802-0fedf8d4b579/image.png)


- 리볼버
  - 작은 세개의 탄환을 발사합니다.
  ![](https://velog.velcdn.com/images/kjhdx/post/c6c65dd7-a4d3-4e67-aa08-81211011f62a/image.png)

  
### 3.3. 공격 / 잡기 / 가드 / 필살기
  
- 공격
  - 좌클릭 / 우클릭을 통해 좌우 각각의 주먹을 발사합니다.
  ![](https://velog.velcdn.com/images/kjhdx/post/48ed75d4-0077-42e5-b522-298c9d1eb06e/image.png)


- 잡기
  - 잡기를 이용한 공격이 가능합니다. 잡기는 가드를 무시합니다.
  ![](https://velog.velcdn.com/images/kjhdx/post/6b7258b1-de94-429f-9365-c8b4bb4ecf48/image.png)

  
- 가드
  - 가드를 통해 공격을 막을 수 있습니다. 가드를 2초이상 하면 충전 상태가 됩니다. 충전 상태에서의 공격은 추가 대미지를 줍니다.
  ![](https://velog.velcdn.com/images/kjhdx/post/5064bb6c-8962-4f8a-8900-50a9e45ddcec/image.png)
  
- 필살기
  - 시간이 지날수록 충전되는 필살기 게이지가 꽉 차면 필살기를 사용할 수 있습니다. 필살기는 각각의 주먹마다 다른 연출 및 성능을 가집니다.
  ![](https://velog.velcdn.com/images/kjhdx/post/eaa33d88-d522-4257-9f33-a130cb09b153/image.png)

### 3.4. 상세 역할
- 저는 플레이어의 이동/대쉬/점프 등의 움직임과 애니메이션 구현 및 카메라 무빙, 피격시 이펙트 및 카메라 쉐이킹 등의 이펙트와 사운드를 작업하였습니다. 또한 AI의 행동 로직 구현 또한 맡았습니다.

- 한명이 3가지 주먹의 공격 및 잡기를 작업하고, 나머지 한명이 가드, 체력, UI를 담당하였습니다.

</div>
</details>

</br>

## 4. 회고 / 느낀점

- 저와 함께 작업한 두명의 팀원은 비전공자였고, 따라서 제가 팀원을 많이 도와주면서 프로젝트를 진행하였습니다. 한명의 플레이어를 3명이 나누어 작업하면서 많은 오류가 있었고, 이를 통해 캡슐화를 통한 데이터 보호 및 분업의 필요성을 느꼈습니다.

- Charactor Controller를 통한 플레이어 움직임 구현에 익숙해졌습니다. 또한 카메라 쉐이킹/이펙트/사운드가 게임에 정말 많은 영향을 준다고 느꼈습니다.
</br>

## 5. 시연 영상
https://www.youtube.com/watch?v=Vhtlnhk3hIE
