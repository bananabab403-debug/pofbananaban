using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
   
   
        
    public partial class MainWindow : Window                          
        {
            private int[] _array;                                       
            private readonly Random _rnd = new Random();                  

            private int[] _viz;                                            
            private int _vizLeft;                                          
        private int _vizRight;                                             
        private int _vizMid;
        private int _vizTarget;                                        

            public MainWindow()
            {
                InitializeComponent();                                    
            }

            private void BtnGenerate_Click(object sender, RoutedEventArgs e)
            {
                if (!int.TryParse(txtSize.Text, out int n) || n < 5 || n > 100000) 
                {
                    lblGenResult.Text = "Ошибка: N от 5 до 100000.";       
                    return;                                                
                }

                string mode = (cmbMode.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Случайный"; 
                _array = new int[n];                                       

                for (int i = 0; i < n; i++)                                
                {
                    if (mode == "Случайный")                               
                        _array[i] = _rnd.Next(0, 10000);                   
                    else if (mode == "Обратный порядок")                   
                        _array[i] = n - i;                        
                    else                                                  
                        _array[i] = i;                                     
                }

                if (mode == "Случайный") Array.Sort(_array);             

                txtArray.Text = n <= 50                                  
                    ? string.Join(" ", _array)                           
                    : $"Массив из {n} элементов сгенерирован.";           

                lblGenResult.Text = "Готово.";  
            }

            private void BtnSearch_Click(object sender, RoutedEventArgs e)
            {
                if (_array == null)                                        
                {
                    lblSearchResult.Text = "Сначала сгенерируйте массив."; 
                    return;                                                
                }

                if (!int.TryParse(txtTarget.Text, out int target))         
                {
                    lblSearchResult.Text = "Ошибка: введите целое число."; 
                    return;                                               
                }

                string algo = (cmbAlgorithm.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Линейный"; 
                var sw = System.Diagnostics.Stopwatch.StartNew();          
                int idx = -1;                                              

                switch (algo)                                              
                {
                    case "Линейный":                                       
                        idx = SearchAlgorithms1.LinearSearch(_array, target);
                        break;
                    case "Бинарный":                                       
                        idx = SearchAlgorithms1.BinarySearch(_array, target);
                        break;
                    case "Интерполяционный":                               
                        idx = SearchAlgorithms1.InterpolationSearch(_array, target);
                        break;
                    case "Хэш-таблица":                                    
                        var table = SearchAlgorithms1.BuildHashTable(_array); 
                        idx = SearchAlgorithms1.HashSearch(table, target);  
                        break;
                }
                sw.Stop();                                                
                lblSearchResult.Text = idx >= 0                            
                    ? $"{algo}: найден на индексе {idx}, время {sw.Elapsed.TotalMilliseconds:F3} мс"
                    : $"{algo}: не найден, время {sw.Elapsed.TotalMilliseconds:F3} мс";
            }
            private void BtnCompare_Click(object sender, RoutedEventArgs e)
            {
                if (_array == null)                                        
                {
                    lblSearchResult.Text = "Сначала сгенерируйте массив."; 
                    return;                                                
                }

                if (!int.TryParse(txtTarget.Text, out int target))         
                {
                    lblSearchResult.Text = "Ошибка: введите целое число."; 
                    return;                                                
                }

                var results = new List<SearchResult>();                    

                var sw1 = System.Diagnostics.Stopwatch.StartNew();         
                int idx1 = SearchAlgorithms1.LinearSearch(_array, target);  
                sw1.Stop();                                                
                results.Add(new SearchResult { Algorithm = "Линейный", Index = idx1, Milliseconds = sw1.Elapsed.TotalMilliseconds }); 

                var sw2 = System.Diagnostics.Stopwatch.StartNew();         
                int idx2 = SearchAlgorithms1.BinarySearch(_array, target);  
                sw2.Stop();                                                
                results.Add(new SearchResult { Algorithm = "Бинарный", Index = idx2, Milliseconds = sw2.Elapsed.TotalMilliseconds }); 

                var sw3 = System.Diagnostics.Stopwatch.StartNew();         
                int idx3 = SearchAlgorithms1.InterpolationSearch(_array, target); 
                sw3.Stop();                                                
                results.Add(new SearchResult { Algorithm = "Интерполяционный", Index = idx3, Milliseconds = sw3.Elapsed.TotalMilliseconds }); 

                var sw4 = System.Diagnostics.Stopwatch.StartNew();         
                var table = SearchAlgorithms1.BuildHashTable(_array);       
                int idx4 = SearchAlgorithms1.HashSearch(table, target);     
                sw4.Stop();                                                
                results.Add(new SearchResult { Algorithm = "Хэш-таблица", Index = idx4, Milliseconds = sw4.Elapsed.TotalMilliseconds }); 

                gridResults.ItemsSource = results;                         
            }

            private void BtnVizReset_Click(object sender, RoutedEventArgs e)
            {
                _viz = new int[20];                                        
                for (int k = 0; k < _viz.Length; k++)                      
                    _viz[k] = (k + 1) * 10;                                
                _vizTarget = _rnd.Next(1, 21) * 10;                        
                _vizLeft = 0;                                              
                _vizRight = _viz.Length - 1;                               
            _vizMid = -1;                                              
            DrawViz();                                                 
            }

            private void BtnVizStep_Click(object sender, RoutedEventArgs e)
            {
                if (_viz == null || _vizLeft > _vizRight)                  
                {
                    DrawViz();                                             
                    return;                                                
                }

                _vizMid = _vizLeft + (_vizRight - _vizLeft) / 2;          

                if (_viz[_vizMid] == _vizTarget)                    
                    _vizLeft = _vizRight + 1;                              
                else if (_viz[_vizMid] < _vizTarget)                      
                    _vizLeft = _vizMid + 1;                                
                else                                                       
                    _vizRight = _vizMid - 1;                              

                DrawViz();                                                 
            }

            private void DrawViz()
            {
                if (canvas == null || _viz == null) return;                

                canvas.Children.Clear();                                   
                double w = canvas.ActualWidth / _viz.Length;               
                if (w <= 0) w = 20;                                        

                for (int k = 0; k < _viz.Length; k++)                      
                {
                    System.Windows.Media.Brush fill;                        
                    if (k == _vizMid) fill = System.Windows.Media.Brushes.OrangeRed;       
                    else if (k >= _vizLeft && k <= _vizRight) fill = System.Windows.Media.Brushes.SteelBlue;
                    else fill = System.Windows.Media.Brushes.LightGray;        

                    var rect = new System.Windows.Shapes.Rectangle         
                    {
                        Width = Math.Max(2, w - 2),                        
                        Height = _viz[k],                                  
                        Fill = fill                                        
                    };

                    Canvas.SetLeft(rect, k * w);                           
                    Canvas.SetTop(rect, canvas.ActualHeight - _viz[k]);   
                    canvas.Children.Add(rect);                             
                }

                var lbl = new System.Windows.Controls.TextBlock            
                {
                    Text = $"Ищем: {_vizTarget}",                          
                    FontWeight = System.Windows.FontWeights.Bold           
                };
                Canvas.SetLeft(lbl, 5);                                    
                Canvas.SetTop(lbl, 5);                                    
                canvas.Children.Add(lbl);                                  
            }
        }

        public class SearchResult                                          
        {
            public string Algorithm { get; set; }                          
            public int Index { get; set; }                                
            public double Milliseconds { get; set; }                        
        }
    }
