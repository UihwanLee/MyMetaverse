# MyMetaverse

## 나만의 메타버스 프로젝트
<br>

<img width="986" height="550" alt="1" src="https://github.com/user-attachments/assets/fd2b28d7-f2ae-4a33-95d9-1aef40cd99c1" />

## 1. 프로젝트 소개

메타버스 공간에서 다양한 미니게임을 즐기고 얻은 Coin으로 캐릭터 커스터마이징 하는 프로젝트입니다.

## 2. 프로젝트 구현 내용

> 필수 과제
 - 캐릭터 이동 및 탐색
 - 맵 설계 및 상호작용 영역
 - 미니 게임 실행
 - 점수 시스템
 - 게임 종료 및 복귀

> 도전 과제
 - 추가 미니 게임
 - 커스텀 캐릭터
 - 리더보드 시스템
 - NPC 대화 시스템

## 3. 사용 기술

> ShaderGraph를 활용한 Character Customizing 기능
<img width="1012" height="719" alt="image" src="https://github.com/user-attachments/assets/f82396a1-4315-4190-a437-d1bcfd5235c6" />

> Generic Singleton을 활용한 미니게임 GameManager 관리
<pre>
<code>
public abstract class MiniGameSingleton<T> : MiniGameBase where T : MiniGameSingleton<T>
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        Instance = (T)this;
    }
}
</code>
</pre>  

> TilePalette를 이용한 Map 관리 
<img width="1281" height="801" alt="image" src="https://github.com/user-attachments/assets/75f1fcbd-f8d8-4cb3-8aa9-542de98a3ccc" />

## 4. 게임 이미지

<img width="985" height="542" alt="3" src="https://github.com/user-attachments/assets/82605b04-1fec-4c2b-b229-9fc1869a26df" />

<img width="990" height="550" alt="2" src="https://github.com/user-attachments/assets/2b30092b-62c4-4e07-9128-102202d2ebff" />

<img width="988" height="547" alt="4" src="https://github.com/user-attachments/assets/50d8f6f9-6622-4f4e-9bd8-f6a7909a74c6" />

<img width="964" height="544" alt="5" src="https://github.com/user-attachments/assets/a5b4ce6e-d4aa-4abf-bb51-f15039e472ec" />



