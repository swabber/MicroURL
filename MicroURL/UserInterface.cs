using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using MicroURLCore;
using System.Threading.Tasks;
using System.Text;

namespace MicroURL {
    /// <summary>
    /// State Machine to interact with user.
    /// </summary>
    internal class UserInterface {
        private enum State { Login, Main, CreateShort, SetShort, DelShort, DelAll, PrintAll, LongFromShort, Logout, Exit }
        private readonly Dictionary<State, Action> StateActions = new();
        private HashSet<char> allowedSymbols = new HashSet<char>() { '$', '-', '_', '.', '+', '!', '*', '’', '(', ')', ',', '.' };
        private State CurrentState { get; set; }
        private MicroURLService MicroURLService { get; set; }
        private string Message {
            get {
                string cur = prevMessage;
                prevMessage = "";
                return cur;
            }
            set { prevMessage = value; }
        }
        private string prevMessage = "";

        internal UserInterface() {
            CurrentState = State.Login;
            StateActions[State.Login] = Login;
            StateActions[State.Main] = MainMenu;
            StateActions[State.CreateShort] = CreateShortUrl;
            StateActions[State.SetShort] = SetShortUrl;
            StateActions[State.DelShort] = DeleteShortUrl;
            StateActions[State.DelAll] = DeleteShortUrlsByLong;
            StateActions[State.PrintAll] = PrintAllShortUrlsByLong;
            StateActions[State.LongFromShort] = GetLongFromShort;
        }

        internal void Interact() {
            while (CurrentState != State.Exit) {
                StateActions[CurrentState]();
            }
        }
        internal void Login() {
            Console.Clear();
            Console.WriteLine(Message);
            Console.WriteLine();
            Console.WriteLine("Welcome to MicroURL service. Please enter user name:");
            string? userName = Console.ReadLine();
            if (string.IsNullOrEmpty(userName)) {
                Message = "enter valid user"; // simulate Authentication ;)
                CurrentState = State.Login;
                return;
            }
            MicroURLService = new MicroURLService(userName);
            Message = $"Login for user {MicroURLService.CurrentUser} successful";
            CurrentState = State.Main;
        }

        internal void MainMenu() {
            Console.Clear();
            Console.WriteLine(Message);
            Console.WriteLine();
            Console.WriteLine($"Welcome {MicroURLService.CurrentUser} to MicroURL service. Place which respect short url. Please, chouse action.");
            Console.WriteLine();
            Console.WriteLine("1. Create short URL.");
            Console.WriteLine("2. Set custom short URL.");
            Console.WriteLine("3. Delete short URL.");
            Console.WriteLine("4. Delete all short URLs.");
            Console.WriteLine("5. Print all short URLs.");
            Console.WriteLine("6. Get original long URL from short one.");
            Console.WriteLine("7. Logout.");
            Console.WriteLine("8. Exit.");
            var userInput = Console.ReadLine();
            if (!int.TryParse(userInput, out int option)) {
                Message = $"Unsuported action [{userInput}]";
                CurrentState = State.Main;
                return;
            }
            switch (option) {
                case 1:
                    CurrentState = State.CreateShort;
                    return;
                case 2:
                    CurrentState = State.SetShort;
                    return;
                case 3:
                    CurrentState = State.DelShort;
                    return;
                case 4:
                    CurrentState = State.DelAll;
                    return;
                case 5:
                    CurrentState = State.PrintAll;
                    return;
                case 6:
                    CurrentState = State.LongFromShort;
                    return;
                case 7:
                    CurrentState = State.Login;
                    MicroURLService.Dispose();
                    return;
                case 8:
                    CurrentState = State.Exit;
                    return;
                default:
                    Message = $"Unsuported action [{userInput}]";
                    CurrentState = State.Main;
                    return;
            }
        }

        private void CreateShortUrl() {
            Console.Clear();
            Console.WriteLine(Message);
            Console.WriteLine();
            Console.WriteLine("Enter your unbearably long url below:");
            string? longUrl = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(longUrl)) {
                Message = "URL need to be longer";
                CurrentState = State.CreateShort;
                return;
            }
            Message = MicroURLService.CreateShortURL(longUrl);
            CurrentState = State.Main;
        }

        private void SetShortUrl() {
            Console.Clear();
            Console.WriteLine(Message);
            Console.WriteLine();
            Console.WriteLine("Enter your unbearably long url below:");
            string? longUrl = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(longUrl)) {
                Message = "URL need to be longer";
                CurrentState = State.SetShort;
                return;
            }
            Console.Clear();
            Console.WriteLine(Message);
            Console.WriteLine();
            Console.WriteLine($"Enter your {MicroURLService.DomainName}[DesiredID]");
            string? shortId = Console.ReadLine();
            if (IsInvalid(shortId)) {
                Message = "unsuported short ID. Please enter enother one.";
                CurrentState = State.SetShort;
                return;
            }
            if (!MicroURLService.SetShortURL(longUrl, shortId)) {
                Message = "Looks like short ID already exist. Please enter enother one.";
                CurrentState = State.SetShort;
                return;
            }
            Message = $"Short url {MicroURLService.DomainName}{shortId} created. Please, try it out.";
            CurrentState = State.Main;
        }

        private bool IsInvalid(string? shortId) {
            if (string.IsNullOrWhiteSpace(shortId) || shortId.Length < 3 || shortId.Length > 12)
                return true;
            foreach (char c in shortId) {
                if (!char.IsLetterOrDigit(c) && !allowedSymbols.Contains(c))
                    return true;
            }
            return false;
        }

        private void DeleteShortUrl() {
            Console.Clear();
            Console.WriteLine(Message);
            Console.WriteLine();
            Console.WriteLine("Enter your short url below to Delete it:");
            string? shortId = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(shortId)) {
                Message = "Empty ID is invalid";
                CurrentState = State.Main;
                return;
            }
            shortId = shortId.Trim().Replace(MicroURLService.DomainName, "");
            if (string.IsNullOrWhiteSpace(shortId)) {
                Message = "Empty ID is invalid";
                CurrentState = State.Main;
                return;
            }
            if (!MicroURLService.DeleteShortURL(shortId)) {
                Message = "Could not find such a record";
                CurrentState = State.Main;
                return;
            }
            Message = "Deleted successful.";
            CurrentState = State.Main;
        }

        private void DeleteShortUrlsByLong() {
            Console.Clear();
            Console.WriteLine(Message);
            Console.WriteLine();
            Console.WriteLine("Enter your long url to delete all relevant short urls");
            string? longUrl = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(longUrl)) {
                Message = "URL need to be longer";
                CurrentState = State.Main;
                return;
            }
            if (!MicroURLService.DeleteAllShortURLs(longUrl)) {
                Message = "Could not find such record";
            }
            CurrentState = State.Main;
        }

        private void PrintAllShortUrlsByLong() {
            Console.Clear();
            Console.WriteLine(Message);
            Console.WriteLine();
            Console.WriteLine("Enter your long url to find all relevant short urls");
            string? longUrl = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(longUrl)) {
                Message = "URL need to be longer";
                CurrentState = State.Main;
                return;
            }
            StringBuilder message = new();
            foreach (string url in MicroURLService.GetAllShortURLs(longUrl)) {
                message.AppendLine(url);
            }
            Message = message.ToString();
            CurrentState = State.Main;
        }
        private void GetLongFromShort() {
            Console.Clear();
            Console.WriteLine(Message);
            Console.WriteLine();
            Console.WriteLine("Enter your short url");
            string? shortId = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(shortId)) {
                Message = "Empty ID is invalid";
                CurrentState = State.Main;
                return;
            }
            shortId = shortId.Trim().Replace(MicroURLService.DomainName, "");
            if (string.IsNullOrWhiteSpace(shortId)) {
                Message = "Empty ID is invalid";
                CurrentState = State.Main;
                return;
            }
            Message = MicroURLService.GetLongFromShortURL(shortId);
            CurrentState = State.Main;
        }
    }
}