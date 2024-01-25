using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library_management_system
{
    internal class Program
    {
        /// <summary>
        /// Consol-based Library Management System developed in C# to allow user make operations
        /// such as add books, show generl and specific views, search books, borrow books and, return books.  
        /// This application use list to store genres, dictionary to store all the book's information
        /// and Queue to store the borrow and returns books.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                //New object - Creating an instance of the class 
                LibraryManagementSystem libraryManageSystem = new LibraryManagementSystem();

                // Changing the backgroundColor color 
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Blue;

                //Show main option menu of the system
                libraryManageSystem.MenuOfOptions();

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nError ocurred! " + ex.Message);
            }
            finally
            {
                Console.WriteLine("\nProgram completed");
                Console.ResetColor();
            }
        }
    }
}
