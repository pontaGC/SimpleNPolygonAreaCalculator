using Microsoft.WindowsAPICodePack.Dialogs;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNPolygonAreaCalculator
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Gets an input file
            var dialogResult = ShowOpenInputFileDialog();
            if (dialogResult.IsCanceled)
            {
                return;
            }

            // Loads positions of n-polygons' verticies
            var allPositions = LoadPositions(dialogResult.FilePath).ToArray();
            if (allPositions.Any() == false)
            {
                Console.WriteLine($"頂点の座標読み取りに失敗しました({dialogResult.FilePath})");
                Console.ReadKey();

                return;
            }

            // Calculate area
            var areaValue = CalculateArea(allPositions);

            Console.WriteLine($"計算結果: {areaValue}");
            Console.WriteLine($"インプット: {dialogResult.FilePath}");

            Console.WriteLine("何かキーを押すと終了します");
            Console.ReadKey();
        }

        [STAThread]
        private static (bool IsCanceled, string FilePath) ShowOpenInputFileDialog()
        {
            // ConsoleAppにUI要素を本当は入れるのよくない

            var dialog = new CommonOpenFileDialog()
            {
                IsFolderPicker = false,
                EnsureFileExists = true,
                EnsurePathExists = true,
                Multiselect = false,
            };

            dialog.Filters.Add(new CommonFileDialogFilter("座標ファイル", ".txt"));

            return dialog.ShowDialog() == CommonFileDialogResult.Ok
                                          ? (false, dialog.FileName)
                                          : (true, string.Empty);
        }

        private static IEnumerable<Position> LoadPositions(string inputFilePath)
        {
            var allLines = File.ReadAllLines(inputFilePath);

            var inputPositions = new List<Position>();

            int numberOfLines = allLines.Length;
            for(int i = 0; i < numberOfLines; ++i)
            {
                var currentLine = allLines[i];
                if (string.IsNullOrWhiteSpace(currentLine))
                {
                    continue;
                }

                var readLineResult = ReadOnePosition(i, currentLine);
                if (readLineResult.IsFailed)
                {
                    return Enumerable.Empty<Position>();
                }

                inputPositions.Add(readLineResult.Position);
            }

            return inputPositions;
        }

        private static (bool IsFailed, Position Position) ReadOnePosition(int lineCount, string readLine)
        {
            // Gets a string separated by whitespace
            var valueStrings = readLine.Split(' ');
            
            if (valueStrings.Length < 2)
            {
                Console.WriteLine($"行{lineCount}: 入力値が少なすぎます");
                return (true, Position.Zero);
            }

            if (double.TryParse(valueStrings[0], out var xValue) == false)
            {
                Console.WriteLine($"行{lineCount}: x座標の値が不正です({valueStrings[0]})");
                return (true, Position.Zero);
            }

            if (double.TryParse(valueStrings[1], out var yValue) == false)
            {
                Console.WriteLine($"行{lineCount}: y座標の値が不正です({valueStrings[1]})");
                return (true, Position.Zero);
            }

            return (false, new Position(xValue, yValue));
        }

        private static double CalculateArea(Position[] positions)
        {
            // https://imagingsolution.net/math/calc_n_point_area/

            double areaSum = MathConstants.Zero;
            for(int j = 0; j < positions.Length; ++j)
            {
                // Pi
                var jPos = j == positions.Length ? positions[0] : positions[j];

                // Pi+1
                var j1Pos = (j + 1) == positions.Length ? positions[0] : positions[j + 1];

                areaSum += (jPos.X * j1Pos.Y) - (j1Pos.X * jPos.Y);
            }

            return MathConstants.Half * Math.Abs(areaSum);
        }
    }
}
