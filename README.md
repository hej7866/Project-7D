# Project : 7D

> 밤마다 좀비가 몰려온다. 수단과 방법을 가리지 않고 좀비를 막아내자.

## 소개

**Project : 7D**는 플레이어가 **자원을 캐며** 전략적인 기지를 구축하는 **좀비 아포칼리스 형태의 3D 디펜스 게임(프로토타입)**이다.

**1인개발 게임**

**개발 기간 : 2025-05 ~ 2025-07**

## 기술스택
- IDE : Visual Studio Code
- Game Engine : Unity 2022.3.14f1
- Language : C#

## 게임 시스템구조 아키텍처
  <img width="1600" height="1500" alt="image" src="https://github.com/user-attachments/assets/17b36bf4-c52e-4756-b2fe-2fac729902e8" />


## 주요기능
- 터레인기반 맵 생성 및 자원배치 기능 (펄린노이즈 알고리즘, 오브젝트 풀링)
  <img width="1067" height="586" alt="image" src="https://github.com/user-attachments/assets/85090096-3ff3-4a54-a02b-d1abefda09c0" />
- 좀비 웨이브 기능 (오브젝트 풀링)
- 좀비-플레이어 추적 기능
- 아이템 기능 (ScriptableObject)
- 상점 기능
- 인벤토리 기능
- 퀵슬롯 기능
- 타워 설치 기능

## 디자인 패턴
- 싱글톤 패턴
- 상태 패턴
- 옵저버 패턴

