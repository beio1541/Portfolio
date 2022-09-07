# Portfolio라고 쓰고 연습용 Unity라고 읽는다.

기초부터 시작하는 프레임워크 다지기

# Framework
 - Framework의의
  1. Framework는 두번 컴파일 하지 않기위해 어셈블리 처리한다.
  2. Framework는 Unity를 사용하기위해 윤활제 역활을 위해서 존재한다.
  3. Framework자체는 별도의 ThirdParty를 가져가지 않는다.
 - Framework의 리스트
  1. UnityEngine에서 지원하지 않는 각종 Helper 추가
  - 1. StringHelper
       - 각종 변수에서 Comma를 추가한 string 변환 기능 추가
       - 해당 string에 Color값을 입히는 rich text용 string 변환
  - 2. ColorHelper
      - 각종 비트 및 HEX를 이용한 Color로 변환
      - Color를 int 혹은 string으로 변환
  - 3. EnumHelper
      - Enum을 각종 변수로 컨버팅 하거나 반대로 변환
  - 4. GameObjectHelper
      - 각종 Component에서 SetActive를 처리 하기 위한 Helper
      - Framework 어셈블리 외부에서 사용하기 위해 partial로 처리 ( 빌드 처리 안해봄 )
  3. 변수 암호화 모듈 제작 (속도가 느림)
   - 1. 각종 변수(int, bool, short, float, string등등)를 String으로 변환, AES로 암호화
   - 2. Reflection으로 해당 변수의 TryParse함수를 검색
   - 3. AES로 복호화 된 string을 TryParse로 변환후 리턴
  5. 네트워크의 기능 정리
   - 1. WebRequest
   - 2. Socket
  
 

# 최신기술에 대한 테스트 및 장단점 파악

# UniRx의 대한 고찰

- 장점
  1. 비동기에 대한 처리가 좀더 편해진다.
  2. 코루틴에 비해 소스 처리가 간편하다.
  3. Task 대체재로 사용 가능하다. (Task는 C# 8.0(Unity 2020.2)부터 지원)

- 단점
 1. ThirdParty로 판단되기때문에 별도의 컴파일(어셈블리처리로 한번은 필요)해야된다.
 2. 2019년 7월 4일 이후 업데이트가 없기때문에 내부 버그는 따로 수정해야된다.
 
