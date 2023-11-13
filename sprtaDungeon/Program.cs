using System.Reflection.PortableExecutable;

namespace sprtaDungeon
{
    internal class Program
    {
        private static Character player;           //캐릭터 객체
        private static List<Item> inventory;        //인벤토리 리스트
        static void Main(string[] args)
        {
            GameDataSetting();            //게임 데이터 초기화
            DisplayGameIntro();             //게임 소개 화면 표시
        }
        //게임 데이터 초기화 메서드
        static void GameDataSetting()
        {
            //캐릭터 정보 세팅
            player = new Character("르탄", "전사", 1, 10, 5, 100, 1500);

            //인벤토리 초기화
            inventory = new List<Item>();
            //아이템 정보 세팅
            AddItem(new Weapon("낡은 검", 2, "오래 못 쓸거같은 낡은 검"));
            AddItem(new Weapon("강철 검", 5, "튼튼하다"));

            AddItem(new Shield("나무 방패", 5, "상점에서 구할수 있는 가장 흔한 방패"));
            AddItem(new Shield("강철 방패", 8, "강철로 만든 방패 튼튼하다"));
        }
        //아이템 추가 메서드
        static void AddItem(Item item)
        {
            inventory.Add(item);
        }

        //게임 소개 화면 표기 메서드
        static void DisplayGameIntro()
        {
            Console.Clear();

            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("1. 상태보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int input = CheckValidInput(1, 2);
            switch (input)
            {
                case 1:
                    DisplayMyInfo();       //상태보기 화면 표시
                    break;

                case 2:
                    DisplayInventory();      //인벤토리 화면 표시
                    break;

            }

            //상태보기 화면 표시 메서드
            static void DisplayMyInfo()
            {
                Console.Clear();

                Console.WriteLine("상태보기");
                Console.WriteLine("캐릭터의 정보르 표시합니다.");
                Console.WriteLine();
                Console.WriteLine($"Lv.{player.Level}");
                Console.WriteLine($"{player.Name}({player.Job})");
                Console.WriteLine($"공격력 :{player.Atk + player.EquippedWeapon?.Atk ?? 0}"+ $"{(player.EquippedWeapon?.Atk > 0 ? $"(+{player.EquippedWeapon.Atk})" : "")}");
                Console.WriteLine($"방어력 :{player.Def+ player.EquippedShield?.Def ?? 0}"+ $"{(player.EquippedShield?.Def > 0 ? $"(+{player.EquippedShield.Def})" : "")}");
                Console.WriteLine($"체력 : {player.Hp}");
                Console.WriteLine($"Gold : {player.Gold} G");
                Console.WriteLine();
                Console.WriteLine("0. 나가기");

                int input = CheckValidInput(0, 0);
                switch (input)
                {
                    case 0:
                        DisplayGameIntro();     //게임 소개 화면으로 돌아가기
                        break;
                }
            }

            //인벤토리 화면 표시 메서드
            static void DisplayInventory()
            {
                Console.Clear();

                Console.WriteLine("인벤토리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");

                for (int i = 0; i < inventory.Count; i++)
                {
                    string itemInfo = "";
                    if (inventory[i] is Weapon)
                    {
                        //무기 정보 표시
                        itemInfo = $"- {i + 1} {(inventory[i].IsEquipped ? "[E]":"")} {inventory[i].Name}   | 공격력: {(inventory[i] as Weapon).Atk} | {inventory[i].Information}";
                    }
                    else if (inventory[i] is Shield)
                    {
                        // 방어구 정보 표시
                        itemInfo = $"- {i + 1} {(inventory[i].IsEquipped ? "[E]" : "")} {inventory[i].Name}   | 방어력: {(inventory[i] as Shield).Def} | {inventory[i].Information}";
                    }
                    Console.WriteLine(itemInfo);
                }
                Console.WriteLine();
                Console.WriteLine($"{inventory.Count +1}. 나가기");


                int input = CheckValidInput(1, inventory.Count +1);

                if (input <= inventory.Count)
                {
                    //아이템을 선택하여 장착 또는 헤제
                    EquipItem(inventory[input-1]);
                }
                else
                {
                    DisplayGameIntro();  //게임 소개 화면으로 돌아가기
                }
            }
            // 아이템을 장착 또는 해제하는 메서드 추가
            static void EquipItem(Item item)
            {
                if (item.IsEquipped)
                {
                    // 아이템이 이미 장착된 경우 해제
                    Console.WriteLine($"[E] {item.Name}을(를) 해제했습니다.");

                    if (item is Weapon)
                    {
                        player.EquippedWeapon = null;
                    }
                    else if (item is Shield)
                    {
                        player.EquippedShield = null;
                    }
                }
                else
                {
                    // 아이템을 장착
                    Console.WriteLine($"[E] {item.Name}을(를) 장착했습니다.");

                    if (item is Weapon) 
                    {
                        player.EquippedWeapon = item as Weapon;
                    }

                    else if(item is Shield)
                    {
                        player.EquippedShield = item as Shield;

                    }
                }
                // 장착 여부를 토글
                item.IsEquipped = !item.IsEquipped;

                // 인벤토리 다시 표시
                DisplayInventory();
            }


        }

        //사용자로부터 입력을 받아 검증하는 메서드
        static int CheckValidInput(int min, int max)
        {
            while (true)
            {
                string input = Console.ReadLine();

                bool parseSuccess = int.TryParse(input, out var ret);
                if (parseSuccess)
                {
                    if (ret >= min && ret <= max)
                        return ret;
                }

                Console.WriteLine("잘못된 입력입니다.");
            }
        }
    }

    // 캐릭터 클래스
    public class Character
    {
        public string Name { get; }
        public string Job { get; }
        public int Level { get; }
        public int Atk { get; }
        public int Def { get; }
        public int Hp { get; }
        public int Gold { get; }
        public Weapon EquippedWeapon { get; set; }   //장착된 무기
        public Shield EquippedShield { get; set; }   //장착된 방어구


        //생성자
        public Character(string name, string job, int level, int atk, int def, int hp, int gold)
        {
            Name = name;
            Job = job;
            Level = level;
            Atk = atk;
            Def = def;
            Hp = hp;
            Gold = gold;
        }
    }

    //아이템 추상 클래스
    public abstract class Item
    {
        public string Name { get; }
        public string Information {  get; }
        public int Atk { get; protected set; }    //아이템의 공격력
        public int Def { get; protected set; }   //아이템의 방어력
        public bool IsEquipped { get; set; }     //장착 여부

        //생성자
        protected Item(string name, string information, int atk, int def)
        {
            Name = name;
            Information = information;
            Atk = atk;
            Def = def;
            IsEquipped = false; //초기값은 장착되지 않음
        }
    }

    //무기 클래스
    public class Weapon : Item
    {

        public Weapon(string name, int atk, string information)
            :base(name,information, atk, 0)
        {
        }
    }

    //방어구 클래스
    public class Shield : Item
    {

        public Shield(string name, int def, string information)
            : base(name, information, 0, def)
        {
        }
    }



}
