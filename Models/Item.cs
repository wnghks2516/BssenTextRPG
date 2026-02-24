namespace TextRPG.Models;

public abstract class Item
{
    #region 프로퍼티
    //아이템 이름

    //protected set; : 외부에서는 읽기만 가능, 상속받은 클래스에서는 수정 가능
    public string Name { get; protected set; }

    //아이템 설명
    public string Description { get; protected set; }

    //아이템 가격
    public int Price { get; protected set; }

    //아이템 타입
    public ItemType Type { get; protected set; }
    #endregion

    #region 생성자
    protected Item(string name, string description, int price, ItemType type)
    {
        Name = name;
        Description = description;
        Price = price;
        Type = type;
    }
    #endregion

    #region 아이템 효과 매서드

    //아이템 사용 추상 매서드
    public abstract bool Use(Player player);

    //아이템 정보 표시
    public virtual void DisplayInfo()
    {
        Console.WriteLine($"=== {Name} ===");
        Console.WriteLine($"설명: {Description}");
        Console.WriteLine($"가격: {Price} 골드");
        Console.WriteLine($"타입: {Type}");
    }
    #endregion
}