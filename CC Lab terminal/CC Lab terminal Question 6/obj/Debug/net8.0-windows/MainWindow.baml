   M S B A M L   `   `   `  ����U  QCC Lab terminal Question 6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:CC_Lab_terminal_Question_6CC_Lab_terminal_Question_6  5   	   +    %CC_Lab_terminal_Question_6.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   :local(clr-namespace:CC_Lab_terminal_Question_6     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      � $��10q�6    � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��0,0,0,10q�6"   .+�5      �� $,��&Enter usernames (separated by commas):��6   $��0,0,0,5q�6F   �� 5      -   5         ��Name$ UsernamesInput��5      $��60��6.    �� TextWrapping$
 Wrap=�6:    �� AcceptsReturn$
 True��6N   �� 5      -   5        $��Validate Usernames��5      $��0,10,0,0q�6O   5      �� 5   
   -   5         ��Name$ ResultsTextBox��5      $��1��6.    �� 
IsReadOnly$
 True��6;   " �� VerticalScrollBarVisibility$
 Auto=�5      $��0,0,0,10q�69   �� 5   
   $��2��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   .+�5      �� -   5        $��Save Results��5      $��0,0,10,0q�6E   �� 5      -   5        $��Retry Invalid��5      6-   5      5      5      