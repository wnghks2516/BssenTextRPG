using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TextRPG.Utils;

namespace TextRPG.Data;

public class GameManager
{
    // 싱글톤 패턴 구현
    // 싱글톤이란 어플리케이션 전체에서 단 하나의 인스턴스만 존재하도록 보장하는 디자인 패턴


    #region 싱글톤 패턴
    //리전이란 클래스, 메서드, 속성 등을 그룹화하여 코드의 가독성과 유지보수성을 높이는 기능
    //싱글톤 인스턴스 ( 내부접근용 변수 : 필드 )

    private static GameManager instance;


    //외부에서 인스턴스에 접근할 수 있는 정적 속성 ( 프로퍼티 )
    public static GameManager Instance
    {
        get
        {
            //인스턴스가 없으면 새로 생성
            if (instance == null)
                instance = new GameManager();
            return instance;
        }
    }

    private GameManager()
    {
        // 클래스가 생성될 때 초기화 작업 수행
    }
    #endregion


    #region 게임 시작/종료

    public void StartGame()
    {
        ConsoleUI.ShowTitle();
        Console.WriteLine("게임을 시작합니다.\n");
    }

    #endregion
}

