   M S B A M L   `   `   `  ����\  XGMap.NET.WindowsForms, Version=1.7.0.0, Culture=neutral, PublicKeyToken=b85b9027b614afef\Bclr-namespace:GMap.NET.WindowsForms;assembly=GMap.NET.WindowsFormsGMap.NET.WindowsForms  5   	   T PGMap.NET.Core, Version=1.7.0.0, Culture=neutral, PublicKeyToken=b85b9027b614afef:-clr-namespace:GMap.NET;assembly=GMap.NET.CoreGMap.NET 5   	   [ WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms 5   	   B >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1 5	   	      WpfApp1.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   RgmfBclr-namespace:GMap.NET.WindowsForms;assembly=GMap.NET.WindowsForms     5   	   <gm-clr-namespace:GMap.NET;assembly=GMap.NET.Core     5   	   Owf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     5   	   'localclr-namespace:WpfApp1     5	   	     � Title$  
MainWindow��5   	   $	��400��6   $	��500��6)   .��5      � ���#000000.+�5   
   ]� .+�5      �� $	��Top=�6   $��30��6.   .+�5      �� -   5         ��Name$ 
LoadButton��5      
�������$��
Load Model��6C   $	��150��6q   $��10q�6}   5      � 5      $��30��6   $	��Top=�6   $��Center=�64   $��Center=�6Q    � RowDefinitions 5      �� 5      $
��Auto �6$    � ColumnDefinitions 5      �� 5      $	��125 �6'   �� 5      $	��125 �6'   �� 5      $	��125 �6'   �� 5      $	��125 �6'   .+�5      	� -   5         	�Name$ radioDrawEllipse��5      $��0��68   $��0��6H   $��Draw Ellipse��6U   
�������$��10q�6�   6�   	� 5      -   5        $ radioDrawPolygon��5      $��1��68   $��0��6H   $��Draw Polygon��6U   
�������$��10q�6�   6�   	� 5      -   5        $ radioAddText��5      $��2��64   $��0��6D   $��Add Text��6Q   
�������$��10q�6{   6�   o� 5      $��3��6   $��0��6'   
�������$��10q�6J   $��Center=�6V   .@�5       m� $��     Controls     ��6   .@�5!      m� -   5        $
��Undo��5!   #   
�������6]   m� 5"      -   5        $
��Redo��5"   #   
�������6]   m� 5#      -   5        $��Clear��5#   #   
�������6_   5$      5%      5&      �� 5'      -   5         ��Name$ slider��5'       � Minimum$ .1Q�6%    � Maximum$ 10Q�62    � Value$ 1Q�6?   $	��Top=�6I   $��10q�6^   �� 5(      $
��Auto=�6   $
��Auto=�6@   .��5)      �� $��Center=�6   $��Center=�66   ._�5*      �� -	   5        $��myCanvas��5*      ���#000000$	��400��6B   $	��200��6N   	 � LayoutTransform	 5+      �� 5,      
 �� ScaleX
 6-   ��  �� ElementName$ slider�� �� Path$ Value� �� ScaleY 5-   !   �� $ slider��$ Value�50      51      52      53      54      55      