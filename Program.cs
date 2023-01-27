using System;

namespace _231026
{
    public enum CREATURETYPE
    {
        None,
        Player,
        Monster
    }
    public enum PLAYERTYPE
    {
        None,
        Kngiht,
        Magician,
        Acher
    }
    public enum MONSTERTYPE
    {
        None,
        Slime,
        Orc,
        Skeleton
    }
    public enum GAMEMODE
    {
        None,
        Lobby,
        Town,
        Field
    }
    class Creature
    {
        private CREATURETYPE type;

        protected Creature(CREATURETYPE type)
        {
            this.type = type;
        }

        public void SetInfo(int hp, int atk)
        {
            this.hp = hp;
            this.atk = atk;
        }
        public int GetHP() { return hp; }
        public int GetAtk() { return atk; }


        public int hp = 0;
        public int atk = 0;
        //protected bool isDead = false;

        public bool IsDead() { return hp <= 0;/*isDead;*/ }
        public void OnDamage(int damage)
        {
            hp -= damage;
            if (hp < 0)
            {
                hp = 0;
                //isDead = true;
            }
        }
    }

    class Player : Creature
    {

        public PLAYERTYPE type = PLAYERTYPE.None;

        protected Player(PLAYERTYPE type) : base(CREATURETYPE.Player)
        {
            this.type = type;
        }

    }
    class Knight : Player
    {
        public Knight() : base(PLAYERTYPE.Kngiht)
        {
            SetInfo(100, 5);
        }
    }
    class Magician : Player
    {
        public Magician() : base(PLAYERTYPE.Magician)
        {
            SetInfo(80, 10);
        }

    }
    class Archer : Player
    {
        public Archer() : base(PLAYERTYPE.Acher)
        {
            SetInfo(90, 7);
        }
    }

    class Monster : Creature
    {

        public MONSTERTYPE type = MONSTERTYPE.None;
        public Monster(MONSTERTYPE type) : base(CREATURETYPE.Monster)
        {
            this.type = type;
        }
    }
    class MonsterSlime : Monster
    {
        public MonsterSlime() : base(MONSTERTYPE.Slime)
        {
            SetInfo(15, 2);
        }
    }
    class MonsterOrc : Monster
    {
        public MonsterOrc() : base(MONSTERTYPE.Orc)
        {
            SetInfo(25, 5);
        }
    }
    class MonsterSkeleton : Monster
    {
        public MonsterSkeleton() : base(MONSTERTYPE.Skeleton)
        {
            SetInfo(20, 10);
        }
    }
    class GameManager
    {
        public GAMEMODE mode = GAMEMODE.None;
        public Player player = null;
        public Monster monster = null;
        private Random rand = new Random();
        public void Process()
        {
            switch (mode)
            {
                case GAMEMODE.Lobby:
                    ProcessLobby();
                    break;
                case GAMEMODE.Town:
                    ProcessTown();
                    break;
                case GAMEMODE.Field:
                    ProcessField();
                    break;
            }
        }
        public void ProcessLobby()
        {
            Console.WriteLine("직업을 선택하세요.");
            Console.WriteLine("[1] 기사");
            Console.WriteLine("[2] 마법사");
            Console.WriteLine("[3] 궁수");

            string strInput = Console.ReadLine();
            switch (strInput)
            {
                case "1":
                    player = new Knight();
                    mode = GAMEMODE.Town;
                    break;
                case "2":
                    player = new Magician();
                    mode = GAMEMODE.Town;
                    break;
                case "3":
                    player = new Archer();
                    mode = GAMEMODE.Town;
                    break;
                default:
                    mode = GAMEMODE.Lobby;
                    break;
            }
        }
        public void ProcessTown()
        {
            Console.WriteLine("마을에 입장 했습니다.");
            Console.WriteLine("[1] 필드로 간다");
            Console.WriteLine("[2] 로비로 돌아간다.");

            string strInput = Console.ReadLine();
            switch (strInput)
            {
                case "1":
                    mode = GAMEMODE.Field;
                    break;
                case "2":
                    mode = GAMEMODE.Lobby;
                    break;

            }
        }
        public void ProcessField()
        {
            Console.WriteLine("필드에 입장 했습니다.");
            Console.WriteLine("[1] 전투 모드 돌입");
            Console.WriteLine("[2] 일정 확률로 마을로 도망");

            string strInput = Console.ReadLine();

            switch (strInput)
            {

                case "1":
                    CreateMonster();
                    ProcessBattle();
                    break;
                case "2":
                    ProcessEscape();
                    break;
            }

        }
        public void CreateMonster()
        {

            int randValue = rand.Next(0, 3/*System.Enum.GetValues(typeof(MONSTERTYPE)).Length - 1*/);
            switch (randValue)
            {
                case 0:
                    monster = new MonsterSlime();
                    Console.WriteLine("슬라임이 나타났습니다.");
                    break;
                case 1:
                    monster = new MonsterOrc();
                    Console.WriteLine("오크가 나타났습니다.");
                    break;
                case 2:
                    monster = new MonsterSkeleton();
                    Console.WriteLine("스켈레톤이 나타났습니다.");
                    break;

            }
        }
        public void ProcessEscape()
        {
            int randValue = rand.Next(0, 101);
            if (randValue <= 55)
                mode = GAMEMODE.Town;
            else
            {
                Console.WriteLine("마을로 돌아가지 못했습니다.");
                CreateMonster();
                ProcessBattle();
            }
        }
        public void ProcessBattle()
        {

            while (true)
            {
                int power = player.GetAtk();
                monster.OnDamage(power);
                if (monster.IsDead())
                {
                    Console.WriteLine("승리했습니다!");
                    Console.WriteLine($"남은체력이 {player.GetHP()}입니다.\n");
                    break;
                }
                power = monster.GetAtk();
                player.OnDamage(power);
                if (player.IsDead())
                {
                    Console.WriteLine("패배했습니다.\n");
                    mode = GAMEMODE.Lobby;
                    break;
                }


            }

        }




    }


    class Program
    {
        static void Main(string[] args)
        {
            GameManager gameManager = new GameManager();
            gameManager.mode = GAMEMODE.Lobby;
            while (true)
            {
                gameManager.Process();
            }
        }
    }
}
