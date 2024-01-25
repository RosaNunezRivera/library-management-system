using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace library_management_system
{
    internal class LibraryManagementSystem
    {
        /// <summary>
        /// Method that display menu options with all the operations in the 
        /// Consol-based Library Management System
        /// Ask user to enter a number that march with the option and
        /// tohought the use switch case calls each method for each option.
        ///  (1) Add  
        ///  (2) Viewing  
        ///  (3) Searching  
        ///  (4) Borrowing  
        ///  (6) Returning
        ///  (6) Exit 
        /// </summary>
        public void MenuOfOptions()
        {
            try
            {
                //Declare variables
                int option=0;
                string optionString="";
                bool isValidInt = false;

                //Declaring nested Dictionary to store the books with 
                //Id, title, author, publication year and Genre.
                Dictionary<string, Dictionary<string, string>> libraryCatalog = new Dictionary<string, Dictionary<string, string>>();

                //Creating a List to store the book's genre 
                List<string> genresList = new List<string>();
               
                //Set initial book's information
                SetBook(libraryCatalog, genresList);

                // Sort the list 
                genresList = genresList.OrderBy(genre => genre).ToList();

                //Do-while to be in a loop while option is not exit
                do
                {
                    do
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("\n  Library Management System");
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine("\n  Option menu:");
                        Console.WriteLine("  -------------");
                        Console.WriteLine("  (1) Add");
                        Console.WriteLine("      Add new book into the system");
                        Console.WriteLine("  (2) Viewing");
                        Console.WriteLine("      See the entire library book catalog or filter books by genre");
                        Console.WriteLine("  (3) Searching");
                        Console.WriteLine("      Search books by title, author, or ID");
                        Console.WriteLine("  (4) Borrowing");
                        Console.WriteLine("      Register a borrow of a book");
                        Console.WriteLine("  (5) Returning");
                        Console.WriteLine("      Register a return of a book");
                        Console.WriteLine("  (6) Exit");

                        //Gettint the string enter by the user and parse if is a integer 
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("\n  Enter an option (1-6): ");
                        Console.ForegroundColor = ConsoleColor.Black;
                        optionString = Console.ReadLine();

                        isValidInt = int.TryParse(optionString, out option);

                        if (!isValidInt || string.IsNullOrEmpty(optionString) || option <= 0 || option > 6)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("  Only numbers are valid, please enter numbers 1-6");
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.WriteLine("\n  Press any key to continue...");
                            Console.ReadKey();
                            Console.Clear();
                        }
                    } while (!isValidInt || option <= 0 || option > 6 || string.IsNullOrEmpty(optionString));

                    //Evaluating each value of the option enter by the user
                    switch (option)
                    {
                        case 1:
                            GetBook(libraryCatalog, genresList);
                            break;
                        case 2:
                            ViewingBooks(libraryCatalog, genresList);
                            break;
                        case 3:
                            Search(libraryCatalog);
                            break;
                        case 4:
                            GetBorrow(libraryCatalog);
                            break;
                        case 5:
                            GetReturn(libraryCatalog);
                            break;
                        case 6:
                            Console.WriteLine("\n  Thanks for use this Library Management System");
                            break;
                        default:
                            break;
                    }
                } while (option != 6);
            }
            catch (Exception ex)
            {
                Console.WriteLine("  Error ocurred in option menu! " + ex.Message);
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Method that allow users to enter a new book get informacion like
        /// name, title, Author, publication and, Genre.
        /// This informacion is store in Nested Dictionary 
        /// </summary>
        public void GetBook(Dictionary<string, Dictionary<string, string>> libraryCatalog, List<string> genresList)
        {
            try
            {
                //Declaring variables to enter a book into the system
                string title = "";
                string author;
                int year;
                string publicationYear;
                string genre;
                string id;
                bool validYear = false;

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\n  Library Management System");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("  Option (1) Add - Add new book into the system");
                Console.WriteLine("  ---------------------------------------------");

                //Asking the identification number avoiding empty or null value
                do
                {
                    Console.Write("  Enter the ID's book: ");
                    id = Console.ReadLine().Trim();

                    if (string.IsNullOrEmpty(id))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("  Please enter a letters and numbers");
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    if (libraryCatalog.ContainsKey(id))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"  The id {id} is already registered in the system,enter another different\n");
                        Console.ForegroundColor = ConsoleColor.Black;
                        id = "";
                    }
                } while (string.IsNullOrEmpty(id));

                //Asking the title or name of the book avoiding empty or null value
                do
                {
                    Console.Write("  Enter a title (name) of the book: ");
                    title = Console.ReadLine().Trim();

                    if (string.IsNullOrEmpty(title))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("  Please, enter the title of the book");
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                } while (string.IsNullOrEmpty(title));

                //Asking for the author of the book avoiding empty or null value
                do
                {
                    Console.Write("  Author of the book: ");
                    author = Console.ReadLine().Trim();

                    if (string.IsNullOrEmpty(author))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("  Please, enter the author of the book");
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                } while (string.IsNullOrEmpty(author));

                //Asking for the publication year of the book, validating if a integer > 0
                do
                {
                    Console.Write("  Publication Year: ");
                    publicationYear = Console.ReadLine().Trim();

                    if (int.TryParse(publicationYear, out year) && year > 0)
                    {
                        validYear = true;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("  Please enter a positive number");
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                } while (!validYear);

                //Method to Ask for a genre
                genre = GetAndRetunrGenres(genresList);

                // Add a book into the system
                AddNewBook(libraryCatalog, id, title, author, publicationYear, genre, "Available");
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Error ocurred the valud enter is invalid: {ex.Message}");
            }
            catch (ArgumentOutOfRangeException ex) 
            {
                Console.WriteLine($"Error ocurred values out of range: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("  Error ocurred ! " + ex.Message);
                Console.WriteLine($"  StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Method to show a list of genres, ask user for the number and return the index 
        /// of the genre choosen in the list 
        /// </summary>
        /// <param name="genresList"></param>
        /// <returns></returns>
        public string GetAndRetunrGenres(List<string> genresList)
        {
            //Declaring variables 
            string genre = "";
            int genreOption;
            string genreOptionString;
            bool isValidInt = false;

            try
            {
                do
                {
                    Console.Write("\n  Genres \n");
                    for (int i = 0; i < genresList.Count; i++)
                    {
                        Console.WriteLine($"  {i + 1}.{genresList[i]}");
                    }

                    //Getting the number of genre in the list 
                    Console.Write("  Choose one genre: ");
                    genreOptionString = Console.ReadLine().Trim();

                    // Get a string and using a Char.TryParse() method to get a boolean value to validate a valid char input
                    isValidInt = int.TryParse(genreOptionString, out genreOption);

                    //Validating the option enter by the user 
                    if (!isValidInt || genreOption <= 0 || (genreOption - 1) > genresList.Count)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"  Please, enter numbers 1-{genresList.Count + 1}");
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        isValidInt = true;
                        genre = genresList[genreOption - 1];
                    }
                } while (!isValidInt);
            }
            catch (Exception ex)
            {
                Console.WriteLine("  Error ocurred! " + ex.Message);
                Console.WriteLine($"  StackTrace: {ex.StackTrace}");
            }
            return genre;
        }

        /// <summary>
        /// Method to add new books in the SD, checking before if a new book does not exist
        /// an after that store it on the directory, and finally bring the feedback to the user. 
        /// </summary>
        /// <param name="libraryCatalog"></param>
        /// <param name="title"></param>
        /// <param name="author"></param>
        /// <param name="year"></param>
        /// <param name="genre"></param>
        public void AddNewBook(Dictionary<string, Dictionary<string, string>> libraryCatalog, string id, string title, string author, string year, string genre, string status)
        {
            try
            {
                // Check if the id' book is already registered 
                if (!libraryCatalog.ContainsKey(id))
                {
                    //Adding the new book' information
                    Dictionary<string, string> bookInformation = new Dictionary<string, string>
                      {
                        { "Title", title },
                        { "Author", author },
                        { "PublicationYear", year },
                        { "Genre", genre },
                        { "Status", status }
                    };

                    //Adding the new book in the SD
                    libraryCatalog.Add(id, bookInformation);

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"\n  The book {title} with id {id} has been added to the system.");
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"\n  The id {id} is already registered in the system,enter another different");
                    Console.ForegroundColor = ConsoleColor.Black;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  Error ocurred trying add a book: " + ex.Message);
                Console.WriteLine($"  StackTrace: {ex.StackTrace}");
            }
            Console.WriteLine("\n  Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        /// <summary>
        /// Method that allow users views the enrire library catalog or show the books of
        /// an specific genre choose by the user.
        /// </summary>
        public void ViewingBooks(Dictionary<string, Dictionary<string, string>> libraryCatalog, List<string> genresList)
        {
            try
            {
                //Declaring variables to enter a book into the system
                int viewOption=0;
                string viewOptionString;
                bool isValidInt = false;

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\n  Library Management System");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("  Option (2) Viewing - See the entire library book catalog or filter books by genre");
                Console.WriteLine("  --------------------------------------------------------------------------");

                do
                {
                    Console.WriteLine("  (1). All Books - Display the entire library catalog");
                    Console.WriteLine("  (2). By Genre. - Display the list of books based on this genre");
                    Console.Write("  Choose an option: ");
                    viewOptionString = Console.ReadLine().Trim();

                    // Get a string and using a Char.TryParse() method to get a boolean value to validate a valid char input
                    isValidInt = int.TryParse(viewOptionString, out viewOption);

                    if (!isValidInt || viewOption <= 0 || viewOption > 2)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"  Please, enter numbers 1 or 2\n");
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        isValidInt = true;
                    }

                    //Evaluating option choosen by the user 
                    switch (viewOption)
                    {
                        case 1:
                            ViewEntireLibraryCatalog(libraryCatalog, genresList);
                            break;
                        case 2:
                            ViewByGenreLibraryCatalog(libraryCatalog, genresList);
                            break;
                    }
                } while (!isValidInt);
            }
            catch (Exception ex)
            {
                Console.WriteLine("  Error ocurred during Option (2) Viewing - sub menu! " + ex.Message);
                Console.WriteLine($"  StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Method to prepare the structure of data to call the method to print the library catalog
        /// </summary>
        public void ViewEntireLibraryCatalog(Dictionary<string, Dictionary<string, string>> LibraryCatalogToPrint, List<string> genresList)
        {
            try
            {
                //Create a sorted the SD to be show order by genre
                var sortedLibraryCatalog = LibraryCatalogToPrint.OrderBy(x => x.Value["Genre"]).ToDictionary(x => x.Key, x => x.Value);

                //Printing the Library Catalog sorted by genre 
                PrintLibraryCatalog(sortedLibraryCatalog, "Entire Library Catalog View");
            }
            catch (Exception ex)
            {
                Console.WriteLine("  Error ocurred during the view! " + ex.Message);
                Console.WriteLine($"  StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Method to prepare the structure of data to call the method that print the books filter 
        /// by the genre choose by the user
        /// </summary>
        /// <param name="libraryCatalog"></param>
        /// <param name="genresList"></param>
        public void ViewByGenreLibraryCatalog(Dictionary<string, Dictionary<string, string>> libraryCatalogToPrint, List<string> genresList)
        {
            try
            {
                //Declaring variables
                string genreToFilter;

                //Ask user by the genre to filter the books
                genreToFilter = GetAndRetunrGenres(genresList);

                //Create a dictionary with the filter by genre choose by the user 
                Dictionary<string, Dictionary<string, string>> filteredDictionary = libraryCatalogToPrint.Where(bookEntry => bookEntry.Value["Genre"] == genreToFilter).ToDictionary(entry => entry.Key, entry => entry.Value);

                //Calling the method to print th SD with book's information
                PrintLibraryCatalog(filteredDictionary, "View of books based on genre: " + genreToFilter);
            }
            catch (Exception ex)
            {
                Console.WriteLine("  Error ocurred during the view by genre! " + ex.Message);
                Console.WriteLine($"  StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Method to print all the  library catalog sorter by genre using a sorted dictionary 
        /// </summary>
        /// <param name="libraryCatalog"></param>
        public void PrintLibraryCatalog(Dictionary<string, Dictionary<string, string>> LibraryCatalogToPrint, string subTitle)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\n  Library Management System");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("  " + subTitle);
                Console.WriteLine("  ----------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("      Id          Book               Author           Publication Year          Genre            Status       ");
                Console.WriteLine("  ----------------------------------------------------------------------------------------------------------------");
                if (LibraryCatalogToPrint.Count > 0)
                {
                    foreach (var bookEntry in LibraryCatalogToPrint)
                    {
                        Console.WriteLine("   "+bookEntry.Key.PadRight(5).Substring(0, 5) + " | " + PrintInformationBookinView(bookEntry.Value));
                    }
                }
                else
                {
                    Console.WriteLine("  There is not any book to show");
                }
                Console.WriteLine("  ----------------------------------------------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine("  Error ocurred! " + ex.Message);
                Console.WriteLine($"  StackTrace: {ex.StackTrace}");
            }
            Console.WriteLine("\n  Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        /// <summary>
        /// Method to print the book's information (id, title, author, publication year and genre) 
        /// from the nested directory to print en view options
        /// </summary>
        /// <param name="bookInformation"></param>
        public string PrintInformationBookinView(Dictionary<string, string> bookInformation)
        {
            StringBuilder strintToPrint = new StringBuilder();
            try
            {
                foreach (var kvp in bookInformation)
                {
                    strintToPrint.Append($"{kvp.Value.PadRight(18).Substring(0, 18)}" + " | ");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  Error ocurred! " + ex.Message);
                Console.WriteLine($"  StackTrace: {ex.StackTrace}");
            }
            return strintToPrint.ToString();
        }

        /// <summary>
        /// Method that allow user to search book by title, author, or ID.
        /// </summary>
        public void Search(Dictionary<string, Dictionary<string, string>> libraryCatalog)
        {
            //Declaring variables to choose a type of search
            int searchOption=0;
            string searchOptionString;
            bool isValidInt = false;
            try
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\n  Library Management System");
                Console.ForegroundColor = ConsoleColor.Black;

                Console.WriteLine("  Option (3) Searching - Search books by title, author, or ID");
                Console.WriteLine("  --------------------------------------------------------------------------");

                //Asking for the genre of the book
                do
                {
                    Console.WriteLine("  (1) Search by title");
                    Console.WriteLine("  (2) Search by author");
                    Console.WriteLine("  (3) Search by ID");
                    Console.Write("  Choose an option: ");
                    searchOptionString = Console.ReadLine().Trim();

                    // Get a string and using a Char.TryParse() method to get a boolean value to validate a valid char input
                    isValidInt = int.TryParse(searchOptionString, out searchOption);

                    if (!isValidInt || searchOption <= 0 || searchOption > 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"  Please, enter numbers 1 to 3\n");
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    
                    //Evaluating option choosen by the user 
                    switch (searchOption)
                    {
                        case 1:
                            GetInputToSearch(libraryCatalog, "Title");
                            break;
                        case 2:
                            GetInputToSearch(libraryCatalog, "Author");
                            break;
                        case 3:
                            GetInputToSearch(libraryCatalog, "id");
                            break;
                    }
                } while (!isValidInt);
            }
            catch (Exception ex)
            {
                Console.WriteLine("  Error ocurred in the search sub menu! " + ex.Message);
                Console.WriteLine($"  StackTrace: {ex.StackTrace}");
            }
            Console.WriteLine("\n  Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        /// <summary>
        /// Method to ask user the target of search
        /// </summary>
        public void GetInputToSearch(Dictionary<string, Dictionary<string, string>> libraryCatalog, string searchType)
        {
            try 
            {
                //Validating if the SD is empty or has null value
                if (libraryCatalog == null || libraryCatalog.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("  The library catalog has not any book registered.");
                    Console.ForegroundColor = ConsoleColor.Black;
                    return;
                }

                //Declaring variables
                string target;

                //Ask the target of the search 
                do
                {
                    Console.Write($"  Enter the {searchType}: ");
                    target = Console.ReadLine().Trim();

                    if (string.IsNullOrWhiteSpace(target))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("  Invalid input. Please enter a non-empty search target.");
                        Console.ForegroundColor = ConsoleColor.Black;
                        return;
                    }
                } while (string.IsNullOrWhiteSpace(target));

                if (searchType == "id")
                {
                    //Searching a book by ID
                    SearchById(libraryCatalog, target);
                }
                else if (searchType == "Author" || searchType == "Title")
                {
                    //Searching a book by ID
                    SearchByAuthorOrTitle(libraryCatalog, target, searchType);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  Error ocurred in the search sub menu! " + ex.Message);
                Console.WriteLine($"  StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Method to search a book by id
        /// </summary>
        /// <param name="libraryCatalog"></param>
        /// <param name="id"></param>
        public void SearchById(Dictionary<string, Dictionary<string, string>> libraryCatalog, string id)
        {
            if (!libraryCatalog.ContainsKey(id))
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"  The book with Id {id} does not exist in the catalog");
                Console.ForegroundColor = ConsoleColor.Black;
            }
            else
            {
                //Display the information of the book founded
                PrintBookInformation(libraryCatalog, id);
            }
        }

        /// <summary>
        /// Method to seach a book by Author or Title input by the user. The author and title and passed like 
        /// parameter and used for the Dictionary with the matched values
        /// </summary>
        /// <param name="libraryCatalog"></param>
        /// <param name="target"></param>
        /// <param name="searchType"></param>
        public void SearchByAuthorOrTitle(Dictionary<string, Dictionary<string, string>> libraryCatalog, string target, string searchType)
        {
            //Declaring variable
            var booksFounded = new Dictionary<string, Dictionary<string, string>>();
            try 
            {
                // Making a search for the author
                booksFounded = libraryCatalog
                  .Where(book => book.Value.ContainsKey(searchType) && book.Value[searchType].Equals(target, StringComparison.OrdinalIgnoreCase))
                  .ToDictionary(book => book.Key, book => book.Value);
               

                //Checking the result of search 
                if (booksFounded.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"  Any books found for the {searchType}: {target}");
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    // Display information for each matching book - interate the 
                    foreach (var book in booksFounded)
                    {
                        //Printing the book information 
                        PrintBookInformation(libraryCatalog, book.Key);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  Error ocurred in the search sub menu! " + ex.Message);
                Console.WriteLine($"  StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Method to allows user to borrow a book. First the aplication ask user the id of the book 
        /// After check if the book is register and the libraryCatalog SD and show the information or 
        /// bring a message that it is not register. Then, after the books is founded the application 
        /// registrer a borrow registed un a List 
        /// </summary>
        /// <param name="libraryCatalog"></param>
        public void GetBorrow(Dictionary<string, Dictionary<string, string>> libraryCatalog) 
        {
            try
            {
                //Declaring variables to enter a book into the system
                string id;

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\n  Library Management System");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("  Option (4) Borrowing - Register a borrow of a book");
                Console.WriteLine("  ---------------------------------------------------");

                //Asking the identification number avoiding empty or null value
                do
                {
                    Console.Write("  Enter the Id's book: ");
                    id = Console.ReadLine().Trim();

                    if (string.IsNullOrEmpty(id))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("  Please enter a letters and numbers");
                        Console.ForegroundColor = ConsoleColor.Black;
                    }

                } while (string.IsNullOrEmpty(id));

                //Searching a book by ID
                SearchById(libraryCatalog, id);
                SetBorrow(libraryCatalog, id);

                Console.WriteLine("\n  Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine("  Error ocurred! " + ex.Message);
                Console.WriteLine($"  StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Method to display the information of the book founded. The Method receive two parameter
        /// Dictionary and id, using this SD and value create other dictionary with the informacion 
        /// of the book with match with this id.
        /// </summary>
        /// <param name="libraryCatalog"></param>
        /// <param name="id"></param>
        public void PrintBookInformation(Dictionary<string, Dictionary<string, string>> libraryCatalog, string id)
        {
            try
            {
                //Getting the book's information of the id typed 
                Dictionary<string, string> bookInfo = libraryCatalog[id];

                // Display the book's information
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\n  Book Information:");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine($"  Id : {id}");

                foreach (var kvp in bookInfo)
                {
                    Console.WriteLine($"  {kvp.Key} : {kvp.Value}");
                }
            }
            catch (KeyNotFoundException)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"The book with ID {id} does not exist in the catalog.");
                Console.ForegroundColor = ConsoleColor.Black;
            }

            catch (Exception ex)
            {
                Console.WriteLine("  Error occurred during printring book's information: " + ex.Message);
            }
        }


        /// <summary>
        /// Method to set the stutu's book when is borrowed 
        /// </summary>
        /// <param name="libraryCatalog"></param>
        /// <param name="id"></param>
        public void SetBorrow(Dictionary<string, Dictionary<string, string>> libraryCatalog, string id)
        {
            try
            {
                //Declaring variables
                const string StatusKey = "Status";

                //Getting the book's information of the id typed 
                Dictionary<string, string> bookInfo = libraryCatalog[id];

                string statusValue = bookInfo[StatusKey];

                if (statusValue == "Available")
                {
                    //Set the value that the book was borrowed
                    bookInfo[StatusKey] = "Borrow";
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"\n  The borrow of the book with id {id} has been registered");
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"\n  The book with id {id} had alredy borrowed! please, try with another id");
                    Console.ForegroundColor = ConsoleColor.Black;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  Error occurred while registering borrow: " + ex.Message);
            }
        }


        /// <summary>
        /// Method to return a borrowd book
        /// </summary>
        /// <param name="libraryCatalog"></param>
        /// <param name="id"></param>
        public void GetReturn(Dictionary<string, Dictionary<string, string>> libraryCatalog)
        {
            try
            {
                //Declaring variables to enter a book into the system
                string id;

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\n  Library Management System");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("  Option (5) Retuning - Register a return of a book");
                Console.WriteLine("  ---------------------------------------------------");

                //Asking the identification number avoiding empty or null value
                do
                {
                    Console.Write("  Enter the Id's book: ");
                    id = Console.ReadLine().Trim();

                    if (string.IsNullOrEmpty(id))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("  Please enter a letters and numbers");
                        Console.ForegroundColor = ConsoleColor.Black;
                    }

                } while (string.IsNullOrEmpty(id));

                if (!libraryCatalog.ContainsKey(id))
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"  The book with Id {id} does not exist in the catalog");
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    //Display the information of the book founded
                    PrintBookInformation(libraryCatalog, id);
                    SetReturn(libraryCatalog, id);

                    Console.WriteLine("\n  Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  Error occurred while registering borrow: " + ex.Message);
            }
        }

        /// <summary>
        /// Method that change the status value of the book when it is returned 
        /// </summary>
        /// <param name="libraryCatalog"></param>
        /// <param name="id"></param>
        public void SetReturn(Dictionary<string, Dictionary<string, string>> libraryCatalog, string id)
        {
            try
            {
                //Declaring variables
                const string StatusKey = "Status";

                //Getting the book's information of the id typed 
                Dictionary<string, string> bookInfo = libraryCatalog[id];

                string statusValue = bookInfo[StatusKey];
                if (statusValue == "Borrow")
                {
                    //Set the value that the book was borrowed
                    bookInfo[StatusKey] = "Available";
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"\n  The return of the book with id {id} has been register");
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"\n  The book with id {id} had alredy returned! please, try with another id");
                    Console.ForegroundColor = ConsoleColor.Black;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  Error occurred while registering a return: " + ex.Message);
            }
        }

        /// <summary>
        /// Method to set initial book's information to use in the application
        /// </summary>
        /// <param name="libraryCatalog"></param>
        public void SetBook(Dictionary<string, Dictionary<string, string>> libraryCatalog, List<string> genres)
        {
            try 
            {
                //Call the method to add book's information  
                AddBooks(libraryCatalog, "01", "The Hound of the Baskervilles", "Arthur Conan Doyle", "1902", "Mystery", "Borrow");
                AddBooks(libraryCatalog, "02", "Gone Girl", "Gillian Flynn", "2012", "Mystery", "Available");
                AddBooks(libraryCatalog, "03", "The Da Vinci Code", "Dan Brown", "2003", "Mystery", "Available");
                AddBooks(libraryCatalog, "04", "Dune", "Frank Herbert", "1965", "Science Fiction", "Available");
                AddBooks(libraryCatalog, "05", "Neuromancer", "William Gibson", "1984", "Science Fiction", "Borrow");
                AddBooks(libraryCatalog, "06", "Ender's Game", "Orson Scott Card", "1985", "Science Fiction", "Borrow");
                AddBooks(libraryCatalog, "07", "The Lord of the Rings", "J.R.R. Tolkien", "1954-1955", "Fantasy", "Borrow");
                AddBooks(libraryCatalog, "08", "Harry Potter and the Sorcerer's Stone", "J.K. Rowling", "1997", "Fantasy", "Borrow");
                AddBooks(libraryCatalog, "09", "A Game of Thrones", "George R.R. Martin", "1996", "Fantasy", "Available");
                AddBooks(libraryCatalog, "10", "Pride and Prejudice", "Jane Austen", "1813", "Romance", "Available");
                AddBooks(libraryCatalog, "11", "Outlander", "Diana Gabaldon", "1991", "Romance", "Available");
                AddBooks(libraryCatalog, "12", "The Fault in Our Stars", "John Green", "2012", "Romance", "Available");
                AddBooks(libraryCatalog, "13", "The Girl with the Dragon Tattoo", "Stieg Larsson", "2005", "Thriller", "Available");
                AddBooks(libraryCatalog, "14", "The Silence of the Lambs", "Thomas Harris", "1988", "Thriller", "Available");
                AddBooks(libraryCatalog, "15", "The Bourne Identity", "Robert Ludlum", "1980", "Thriller", "Available");
                AddBooks(libraryCatalog, "16", "The Book Thief", "Markus Zusak", "2005", "Historical Fiction", "Available");
                AddBooks(libraryCatalog, "17", "All the Light We Cannot See", "Anthony Doerr", "2014", "Historical Fiction", "Available");
                AddBooks(libraryCatalog, "18", "The Pillars of the Earth", "Ken Follett", "1989", "Historical Fiction", "Available");
                AddBooks(libraryCatalog, "19", "Treasure Island", "Robert Louis Stevenson", "1883", "Adventure", "Available");
                AddBooks(libraryCatalog, "20", "Jurassic Park", "Michael Crichton", "1990", "Adventure", "Available");
                AddBooks(libraryCatalog, "21", "The Adventures of Tintin", "Hergé", "1929-1976", "Adventure", "Available");
                AddBooks(libraryCatalog, "22", "Dracula", "Bram Stoker", "1897", "Horror", "Available");
                AddBooks(libraryCatalog, "23", "The Shining", "Stephen King", "1977", "Horror", "Available");
                AddBooks(libraryCatalog, "24", "Frankenstein", "Mary Shelley", "1818", "Horror", "Available");
                AddBooks(libraryCatalog, "25", "Sapiens: A Brief History of Humankind", "Yuval Noah Harari", "2014", "Non-fiction", "Available");
                AddBooks(libraryCatalog, "26", "The Diary of a Young Girl", "Anne Frank", "1947", "Non-fiction", "Available");
                AddBooks(libraryCatalog, "27", "The Immortal Life of Henrietta Lacks", "Rebecca Skloot", "2010", "Non-fiction", "Available");
                AddBooks(libraryCatalog, "29", "Steve Jobs", "Walter Isaacson", "2011", "Biography", "Available");
                AddBooks(libraryCatalog, "30", "The Glass Castle", "Jeannette Walls", "2005", "Biography", "Available");
                AddBooks(libraryCatalog, "31", "The 7 Habits of Highly Effective People", "Stephen R. Covey", "1989", "Self-help", "Available");
                AddBooks(libraryCatalog, "32", "How to Win Friends and Influence People", "Dale Carnegie", "1936", "Self-help", "Available");
                AddBooks(libraryCatalog, "33", "Atomic Habits", "James Clear", "2018", "Self-help", "Available");
                AddBooks(libraryCatalog, "34", "Catch-22", "Joseph Heller", "1961", "Humor", "Available");
                AddBooks(libraryCatalog, "35", "The Hitchhiker's Guide to the Galaxy", "Douglas Adams", "1979", "Humor", "Available");
                AddBooks(libraryCatalog, "36", "Bridget Jones's Diary", "Helen Fielding", "1996", "Humor", "Available");
                AddBooks(libraryCatalog, "37", "The Waste Land", "T.S. Eliot", "1922", "Poetry", "Available");
                AddBooks(libraryCatalog, "38", "Milk and Honey", "Rupi Kaur", "2014", "Poetry", "Available");
                AddBooks(libraryCatalog, "39", "Leaves of Grass", "Walt Whitman", "1855", "Poetry", "Available");
                AddBooks(libraryCatalog, "40", "Charlotte's Web", "E.B. White", "1952", "Children's Literature", "Available");
                AddBooks(libraryCatalog, "41", "Where the Wild Things Are", "Maurice Sendak", "1963", "Children's Literature", "Available");
                AddBooks(libraryCatalog, "42", "The Adventures of Sherlock Holmes", "Arthur Conan Doyle", "1891", "Mystery", "Borrow");
                //Adding generes to use in the application
                genres.Add("Mystery");
                genres.Add("Science Fiction");
                genres.Add("Fantasy");
                genres.Add("Romance");
                genres.Add("Thriller");
                genres.Add("Thriller");
                genres.Add("Historical Fiction");
                genres.Add("Adventure");
                genres.Add("Horror");
                genres.Add("Non-fiction");
                genres.Add("Biography");
                genres.Add("Self-help");
                genres.Add("Humor");
                genres.Add("Poetry");
                genres.Add("Children's Literature");
                genres.Add("Science");
                genres.Add("Cook");
            }
            catch (Exception ex)
            {
                Console.WriteLine("  Error occurred! " + ex.Message);
            }
        }
        /// <summary>
        /// Method that add the setted information to the Dictionary called libraryCatalog
        /// </summary>
        /// <param name="libraryCatalog"></param>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="author"></param>
        /// <param name="year"></param>
        /// <param name="genre"></param>
        public void AddBooks(Dictionary<string, Dictionary<string, string>> libraryCatalog, string id, string title, string author, string year, string genre, string status)
        {
            try
            {
                // Check if the book is already registered 
                if (!libraryCatalog.ContainsKey(id))
                {
                    Dictionary<string, string> bookInformation = new Dictionary<string, string>
                      {
                        { "Title", title },
                        { "Author", author },
                        { "PublicationYear", year },
                        { "Genre", genre },
                        { "Status", status }
                    };
                    //Addig the book 
                    libraryCatalog.Add(id, bookInformation);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  Error ocurred! " + ex.Message);
                Console.WriteLine($"  StackTrace: {ex.StackTrace}");
            }
        }
    }
}