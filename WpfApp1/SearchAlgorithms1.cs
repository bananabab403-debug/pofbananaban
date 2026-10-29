using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    internal class SearchAlgorithms1
    {
         
         
        public static int LinearSearch(int[] arr, int target)
        {
            for (int i = 0; i < arr.Length; i++)                       
                if (arr[i] == target) return i;                       
            return -1;                                                 
        }

       
        public static int BinarySearch(int[] arr, int target)
        {
            int left = 0;                                             
            int right = arr.Length - 1;                               
            while (left <= right)                                      
            {
                int mid = left + (right - left) / 2;                  
                if (arr[mid] == target) return mid;                 
                if (arr[mid] < target) left = mid + 1;                 
                else right = mid - 1;                                  
            }
            return -1;                                                 
        }

       
        public static int InterpolationSearch(int[] arr, int target)
        {
            int left = 0;                                             
            int right = arr.Length - 1;                                
            while (left <= right && target >= arr[left] && target <= arr[right]) 
            {
                if (left == right)                                    

                if (arr[right] == arr[left])                          
                    return arr[left] == target ? left : -1;            

                int pos = left + (int)((long)(target - arr[left]) * (right - left) / (arr[right] - arr[left]));
                if (pos < left || pos > right) return -1;              

                if (arr[pos] == target) return pos;                    
                if (arr[pos] < target) left = pos + 1;                 
                else right = pos - 1;                                  
            }
            return -1;                                                 
        }

        
        public static Dictionary<int, int> BuildHashTable(int[] arr)
        {
            var table = new Dictionary<int, int>();                    
            for (int i = 0; i < arr.Length; i++)                       
                if (!table.ContainsKey(arr[i]))                        
                    table[arr[i]] = i;
            return table;                                              
        }

        
        public static int HashSearch(Dictionary<int, int> table, int target)
        {
            return table.TryGetValue(target, out int idx) ? idx : -1; 
        }

        
        public static int JumpSearch(int[] arr, int target)
        {
            int n = arr.Length;                                        
            int step = (int)Math.Sqrt(n);                            
            int prev = 0;                                              
            while (arr[Math.Min(step, n) - 1] < target)                
            {
                prev = step;                                          
                step += (int)Math.Sqrt(n);                             
                if (prev >= n) return -1;                              
            }
            while (arr[prev] < target)                                 
            {
                prev++;                                                
                if (prev == Math.Min(step, n)) return -1;             
            }
            return arr[prev] == target ? prev : -1;                    
        }
    }
}

    

