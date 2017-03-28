// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Sammy Guergachi">
//   Sammy Guergachi 2017
// </copyright>
// <summary>
//   Defines the Program type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NudgeHarvester
{
    using System.Windows.Forms;

    using NudgeHarvester;

    using Timer = System.Threading.Timer;

    /// <summary>
    /// The program.
    /// </summary>
    public class Program
    {
        private static NudgeHarvesterForm form = new NudgeHarvesterForm();

        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.Run(form);

        }

    }

}
