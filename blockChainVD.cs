//hash-verify-sign trong c#
RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
string publicPrivateKeyXML = rsa.ToXmlString(true);

// Console.WriteLine("private key là:{0}", publicPrivateKeyXML);
string publicOnlyKeyXML = rsa.ToXmlString(false);
// Console.WriteLine("public key là:{0}", publicOnlyKeyXML);

SHA256 sha256 = SHA256.Create();
byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes("hello world");
//ASCIIEncoding myAscii = new ASCIIEncoding();
//byte[] inputBytes = myAscii.GetBytes("hello world");
Console.WriteLine("input byte là:{0}", inputBytes);
// byte[] outputBytesHash = sha256.ComputeHash(inputBytes);
SHA1Managed hash = new SHA1Managed();
byte[] outputBytesHash = hash.ComputeHash(inputBytes);
Console.WriteLine("output byte là:{0}", outputBytesHash);
String hashed = Convert.ToBase64String(outputBytesHash);
Console.WriteLine("hashed là:{0}", hashed);
RSACryptoServiceProvider rsaCSP = new RSACryptoServiceProvider();
  // byte[] signedd = rsaCSP.SignHash(outputBytesHash, CryptoConfig.MapNameToOID("SHA1"));

//byte[] signedd = rsaCSP.SignHash(outputBytesHash, CryptoConfig.MapNameToOID(publicOnlyKeyXML)); //signed bằng private hay public key thì khi verify đều ra true
byte[] signedd = rsaCSP.SignHash(outputBytesHash, CryptoConfig.MapNameToOID(publicPrivateKeyXML));
Console.WriteLine("đã sign:{0}", signedd);
Console.WriteLine("verify là:{0}", rsaCSP.VerifyHash(outputBytesHash, CryptoConfig.MapNameToOID(publicOnlyKeyXML), signedd));
Console.WriteLine("hash lại :{0}", Convert.ToBase64String(hash.ComputeHash(inputBytes)));
//-----output:
// input byte là:System.Byte[]
// output byte là:System.Byte[]
// hashed là:Kq5sNclPz7QV2+lfQIuc6R7oRu0=
// dã sign:System.Byte[]
// verify là:True
// hash l?i :Kq5sNclPz7QV2+lfQIuc6R7oRu0=   
/*
//======-------------
//Transaction.cs
using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainDemo
{
    public class Transaction
    {
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public int Amount { get; set; }

        public Transaction(string fromAddress, string toAddress, int amount)
        {
            FromAddress = fromAddress;
            ToAddress = toAddress;
            Amount = amount;
        }
    }
}


//Block.cs
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BlockchainDemo
{
    public class Block
    {
        public int Index { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public IList<Transaction> Transactions { get; set; }
        public int Nonce { get; set; } = 0;

        public Block(DateTime timeStamp, string previousHash, IList<Transaction> transactions)
        {
            Index = 0;
            TimeStamp = timeStamp;
            PreviousHash = previousHash;
            Transactions = transactions;
        }

        public string CalculateHash()
        {
            SHA256 sha256 = SHA256.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash ?? ""}-{JsonConvert.SerializeObject(Transactions)}-{Nonce}");
            byte[] outputBytes = sha256.ComputeHash(inputBytes);

            return Convert.ToBase64String(outputBytes);
        }

        public void Mine(int difficulty)
        {
            var leadingZeros = new string('0', difficulty);
            while (this.Hash == null || this.Hash.Substring(0, difficulty) != leadingZeros)
            {
                this.Nonce++;
                this.Hash = this.CalculateHash();
            }
        }
    }
}


//Blockchain.cs
using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainDemo
{
    public class Blockchain
    {
        public IList<Transaction> PendingTransactions = new List<Transaction>();
        public IList<Block> Chain { set;  get; }
        public int Difficulty { set; get; } = 2;
        public int Reward = 1; //1 cryptocurrency

        public Blockchain()
        {
            
        }


        public void InitializeChain()
        {
            Chain = new List<Block>();
            AddGenesisBlock();
        }

        public Block CreateGenesisBlock()
        {
            Block block = new Block(DateTime.Now, null, PendingTransactions);
            block.Mine(Difficulty);
            PendingTransactions = new List<Transaction>();
            return block;
        }

        public void AddGenesisBlock()
        {
            Chain.Add(CreateGenesisBlock());
        }
        
        public Block GetLatestBlock()
        {
            return Chain[Chain.Count - 1];
        }

        public void CreateTransaction(Transaction transaction)
        {
            PendingTransactions.Add(transaction);
        }
        public void ProcessPendingTransactions(string minerAddress)
        {
            Block block = new Block(DateTime.Now, GetLatestBlock().Hash, PendingTransactions);
            AddBlock(block);

            PendingTransactions = new List<Transaction>();
            CreateTransaction(new Transaction(null, minerAddress, Reward));
        }

        public void AddBlock(Block block)
        {
            Block latestBlock = GetLatestBlock();
            block.Index = latestBlock.Index + 1;
            block.PreviousHash = latestBlock.Hash;
            //block.Hash = block.CalculateHash();
            block.Mine(this.Difficulty);
            Chain.Add(block);
        }

        public bool IsValid()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                Block currentBlock = Chain[i];
                Block previousBlock = Chain[i - 1];

                if (currentBlock.Hash != currentBlock.CalculateHash())
                {
                    return false;
                }

                if (currentBlock.PreviousHash != previousBlock.Hash)
                {
                    return false;
                }
            }
            return true;
        }

        public int GetBalance(string address)
        {
            int balance = 0;

            for (int i = 0; i < Chain.Count; i++)
            {
                for (int j = 0; j < Chain[i].Transactions.Count; j++)
                {
                    var transaction = Chain[i].Transactions[j];

                    if (transaction.FromAddress == address)
                    {
                        balance -= transaction.Amount;
                    }

                    if (transaction.ToAddress == address)
                    {
                        balance += transaction.Amount;
                    }
                }
            }

            return balance;
        }
    }
}


//P2PClient.cs
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp;

namespace BlockchainDemo
{
    public class P2PClient
    {
        IDictionary<string, WebSocket> wsDict = new Dictionary<string, WebSocket>();

        public void Connect(string url)
        {
            if (!wsDict.ContainsKey(url))
            {
                WebSocket ws = new WebSocket(url);
                ws.OnMessage += (sender, e) => 
                {
                    if (e.Data == "Hi Client")
                    {
                        Console.WriteLine(e.Data);
                    }
                    else
                    {
                        Blockchain newChain = JsonConvert.DeserializeObject<Blockchain>(e.Data);
                        if (newChain.IsValid() && newChain.Chain.Count > Program.PhillyCoin.Chain.Count)
                        {
                            List<Transaction> newTransactions = new List<Transaction>();
                            newTransactions.AddRange(newChain.PendingTransactions);
                            newTransactions.AddRange(Program.PhillyCoin.PendingTransactions);

                            newChain.PendingTransactions = newTransactions;
                            Program.PhillyCoin = newChain;
                        }
                    }
                };
                ws.Connect();
                ws.Send("Hi Server");
                ws.Send(JsonConvert.SerializeObject(Program.PhillyCoin));
                wsDict.Add(url, ws);
            }
        }

        public void Send(string url, string data)
        {
            foreach (var item in wsDict)
            {
                if (item.Key == url)
                {
                    item.Value.Send(data);
                }
            }
        }

        public void Broadcast(string data)
        {
            foreach (var item in wsDict)
            {
                item.Value.Send(data);
            }
        }

        public IList<string> GetServers()
        {
            IList<string> servers = new List<string>();
            foreach (var item in wsDict)
            {
                servers.Add(item.Key);
            }
            return servers;
        }

        public void Close()
        {
            foreach (var item in wsDict)
            {
                item.Value.Close();
            }
        }
    }
}

//P2PServer.cs
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace BlockchainDemo
{
    public class P2PServer: WebSocketBehavior
    {
        bool chainSynched = false;
        WebSocketServer wss = null;

        public void Start()
        {
            wss = new WebSocketServer($"ws://127.0.0.1:{Program.Port}");
            wss.AddWebSocketService<P2PServer>("/Blockchain");
            wss.Start();
            Console.WriteLine($"Started server at ws://127.0.0.1:{Program.Port}");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.Data == "Hi Server")
            {
                Console.WriteLine(e.Data);
                Send("Hi Client");
            }
            else
            {
                Blockchain newChain = JsonConvert.DeserializeObject<Blockchain>(e.Data);

                if (newChain.IsValid() && newChain.Chain.Count > Program.PhillyCoin.Chain.Count)
                {
                    List<Transaction> newTransactions = new List<Transaction>();
                    newTransactions.AddRange(newChain.PendingTransactions);
                    newTransactions.AddRange(Program.PhillyCoin.PendingTransactions);

                    newChain.PendingTransactions = newTransactions;
                    Program.PhillyCoin = newChain;
                }

                if (!chainSynched)
                {
                    Send(JsonConvert.SerializeObject(Program.PhillyCoin));
                    chainSynched = true;
                }
            }
        }
    }
}
//Program.cs
using Newtonsoft.Json;
using System;

namespace BlockchainDemo
{
    class Program
    {
        public static int Port = 0;
        public static P2PServer Server = null;
        public static P2PClient Client = new P2PClient();
        public static Blockchain PhillyCoin = new Blockchain();
        public static string name = "Unknown";

        static void Main(string[] args)
        {
            PhillyCoin.InitializeChain();

            if (args.Length >= 1)
                Port = int.Parse(args[0]);
            if (args.Length >= 2)
                name = args[1];

            if (Port > 0)
            {
                Server = new P2PServer();
                Server.Start();
            }
            if (name != "Unkown")
            {
                Console.WriteLine($"Current user is {name}");
            }

            Console.WriteLine("=========================");
            Console.WriteLine("1. Connect to a server");
            Console.WriteLine("2. Add a transaction");
            Console.WriteLine("3. Display Blockchain");
            Console.WriteLine("4. Exit");
            Console.WriteLine("=========================");

            int selection = 0;
            while (selection != 4)
            {
                switch (selection)
                {
                    case 1:
                        Console.WriteLine("Please enter the server URL");
                        string serverURL = Console.ReadLine();
                        Client.Connect($"{serverURL}/Blockchain");
                        break;
                    case 2:
                        Console.WriteLine("Please enter the receiver name");
                        string receiverName = Console.ReadLine();
                        Console.WriteLine("Please enter the amount");
                        string amount = Console.ReadLine();
                        PhillyCoin.CreateTransaction(new Transaction(name, receiverName, int.Parse(amount)));
                        PhillyCoin.ProcessPendingTransactions(name);
                        Client.Broadcast(JsonConvert.SerializeObject(PhillyCoin));
                        break;
                    case 3:
                        Console.WriteLine("Blockchain");
                        Console.WriteLine(JsonConvert.SerializeObject(PhillyCoin, Formatting.Indented));
                        break;

                }

                Console.WriteLine("Please select an action");
                string action = Console.ReadLine();
                selection = int.Parse(action);
            }

            Client.Close();
        }
    }
}
*/