   M S B A M L   `   `   `  ����A  =CCLab9, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:CCLab14CCLab14  5   	       CCLab14.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:CCLab14     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      � $��10q�6    � RowDefinitions 5
   
   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� -   5         ��Name$ inputTextBox��5      $��0��5       �� AcceptsReturn$
 True��5       �� TextWrapping$
 Wrap=�5      " �� VerticalScrollBarVisibility$
 Auto=�5      $��0,0,0,10q�5      �� 5   
   -   5         ��Name$ compileButton��5      $��1��5      $��Compile��5      $
��10,5q�5      $��Center=�5      �� 5   
   -   5        $ outputTextBox��5      $��2��5       �� 
IsReadOnly$
 True��5       $
 Wrap=�5!      $
 Auto=�5"      $��0,10,0,0q�5#      5$      5&      