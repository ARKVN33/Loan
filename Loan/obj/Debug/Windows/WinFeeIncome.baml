   M S B A M L   `   `   `  ����U  QPersianDateControls, Version=1.2.4495.22344, Culture=neutral, PublicKeyToken=nullbDclr-namespace:Arash.PersianDateControls;assembly=PersianDateControlsArash.PersianDateControls  5   	    Loan   Loan.Windows.WinFeeIncome   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   dpersianDateControlsDclr-namespace:Arash.PersianDateControls;assembly=PersianDateControls     5   	   -   5        $	��524��5   	   $	��695��6     � WindowStartupLocation$  CenterScreen=�6"    � WindowStyle$
 None=�6G   $��20(�6Z    � 
ResizeMode$ NoResize=�6h   $��	B Nazanin)�6~   ���	#FF192D6E$��2q�6�    � Triggers 5
      :� 5   
    :� RoutedEvent$ FrameworkElement.Loaded��6   .[�5      �� .��5      �� .��5      W�  �� TargetProperty$ Opacity�6&    W� From$ 0X�6J   	 W� To$ 1X�6S    g� Duration$ 0:0:.5D�6Z   	 g� FillBehavior$	 HoldEnd=�6l   5      5      5      .��5      � $��RightToLeft=�6   ���	#FFEEEEF2.+�5   
   �� -   5        ���	#FF192D6E$��0��5   '   ���	#FFEEEEF2$��30��6a   $	��Top=�6m   �� 5   
   -   5        
 ��Name$
 LblTitle��5      $&�� درآمد/هزینه صندوق��6"   $��22(�6>   $
��Bold"�6L   $��0q�6^   $	��Top=�6i      LabelStyle1#���   6�   $��30��6�   $��Center=�6�   �� 5   
   -   5         ��Name$ BtnClose��5      $��×��6#   $��35��6/   $��30��6:   $��22(�6F   $��Arial)�6T   $
��Left=�6u   $	��Top=�6�     ButtonStyle2#���  6�   �� 5   
   -   5        $ BtnMinimize��5      $��-��6&   $��35,0,0,0q�62   $��35��6D   $��30��6O   $��27(�6[   $��Arial)�6i   $
��Left=�6�   $	��Top=�6�     ButtonStyle3#���  6�   �� 5   
   ���	#FF192D6E$��2q�6)   $��10,35,10,10q�6=    �� CornerRadius$ 10��6R   �� 5   
   -   5         ��Name$ 	GroupBox1��5      $��درآمد/هزینه��6&   $
��Bold"�6;   $��	20,45,0,0q�6M   $��RightToLeft=�6`   ���	#FF192D6E$
��Left=�6�   $	��651��6�   $	��200��6�   $	��Top=�6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ DgdFeeIncome��5      $��
20,250,0,0q�6)     DataGridStyle1#���  6=   ���	#FF192D6E$��2q�6�      AreRowDetailsFrozen .    
IsReadOnly .    GridLinesVisibility$ 
Horizontal=�6�      HorizontalGridLinesBrush �	#FF192D6E   AlternatingRowBackground �	#FFF1F4F7 �� AlternationCount$ 2��6a     SelectionMode$ Single=�6v     AutoGenerateColumns .  -  'System.Windows.Input.KeyboardNavigation   TabNavigation$
 Once=�6�     CanUserSortColumns .  $
��Left=�6  $	��651��68  $	��215��6D  $	��Top=�6Q  ��6f   Z�    Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ نوع��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$	 120 62   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6>   �� *FeeIncomeType+   ElementStyle 5       �� 5!       �� 
TargetType" ��6    .�5"      ��  �� Property � ��  �� Value
  �   �6;   5#      5%        5&      $ 
تاریخ��6%   $	 140 64    6@   �� *FeeIncomeDate+ 5'      �� 5(      " ��6    .�5)      ��  � ��
  �OO/�6;   5*      5,        5.      $ مبلغ��6%   $	 160 63    6?   �� *FeeIncomeAmount+! �� StringFormat$! {0:N0}�� 5/      �� 50      " ��6    .�51      ��  � ��
  �iii�6;   52      54        55      $ توضیحات��6%   $	 180 66    6B   �� *FeeIncomeDescription+ 56      �� 57      " ��6    .�58      ��  � ��
  �����6;   59      5;      5=      �� 5>   
   -   5        $
 	LblAmount��5>      $��	مبلغ:��6#   $
��Left=�63   $��389,115,0,0q�6N   $	��Top=�6c   $��30��6{   #���   6�   �� 5?   
   -	   5        $
 LblDescription��5?      $��توضیحات:��6(   $
��Left=�6;   $��
44,185,0,0q�6V   $	��Top=�6j   $��30��6�   #���   6�   �� 5@   
   -
   5        " ��Name$" 	TxtAmount��5@      $��435,115,0,0q�6&   # �� TextWrapping$
# Wrap=�6;     TextBoxStyle1#���  6O   $��6��6w   $ �� 	MaxLength$$ 17��6�   $��30��6�   $	��Top=�6�   $
��Left=�6(  $	��226��6C  �� 5A   
   -   5        $" TxtDescription��5A      $��120,185,0,0q�6*   $
# Wrap=�6?   #���  6S   $��7��6{   $��50��6�   $	��Top=�6�   $
��Left=�6�   $	��541��6�   �� 5C   
   -   5        $ BtnAdd��5C      $��ثبت��6!   $��
98,470,0,0q�6/     ButtonStyle1#���  6C   $
��Left=�6   $	��120��6�   $��7��6�   % �� 	IsDefault% . $��30��6�   $	��Top=�6�   �� 5D   
   -   5        $ BtnNew��5D      $��جدید��6!   $��473,470,98,0q�60   #���  6F   $��30��6�   $	��Top=�6�   $��8��6�   �� 5E   
   -   5        $ BtnEdit��5E      $��ویرایش��6"   $��223,470,0,0q�63   #���  6H   $��30��6o   $	��Top=�6{   $��9��6�   $
��Left=�6�   $	��120��6�   �� 5F   
   -   5        $ 	BtnDelete��5F      $��حذف��6$   $��348,470,0,0q�62   #���  6G   $��7��6n   $
��Left=�6{   $	��120��6�   $��30��6�   $	��Top=�6�   �� 5H   
   -   5        $
 LblRial��5H      $��ریال��6!   $
��Left=�60   $��625,115,0,0q�6K   $	��Top=�6`   $��30��6x   #���   6�   �� 5I   
   -   5        $
 LblFeeIncomeType��5I      $��نوع:��6*   $
��Left=�69   $��	82,80,0,0q�6T   $	��Top=�6g   $��30��6   #���   6�   �� 5J   
   -   5        $
 LblReceiptNumber��5J      $��شماره سند:��6*   $
��Left=�6?   $��350,150,0,0q�6Z   $	��Top=�6o   $��30��6�   #���   6�   �� 5K   
   -   5        $" TxtReceiptNumber��5K      $��30��6,   $��435,150,0,0q�68   $
# Wrap=�6M   $	��Top=�6b   #���  6z   $��6��6�   $$ 15��6�   $
��Left=�6�   $	��226��6�   �� 5L   
   -   5        & ��Name$& CboFeeIncomeType��5L      $
��Left=�6-   $��
120,80,0,0q�6H   $	��Top=�6\   $	��225��6t   $��30��6�   $��5��6�   .@�5M      �� $�� ��6   ' �� 
IsSelected' . 6&   �� 5N      $��
درآمد��6   �� 5O      $��
هزینه��6   5P      �� 5Q   
   -   5        $
 
LblPayType��5Q      $��نوع پرداخت:��6$   $
��Left=�6:   $��
30,150,0,0q�6U   $	��Top=�6i   $��30��6�   #���   6�   �� 5R   
   -   5        $& 
CboPayType��5R      $
��Left=�6'   $��120,150,0,0q�6B   $	��Top=�6W   $	��225��6o   $��30��6{   $��5��6�   .@�5S      �� $�� ��6   ' . 6&   �� 5T      $��نقدی��6   �� 5U      $
��چک��6   �� 5V      $��کارت خوان��6   �� 5W      $��فیش بانکی��6   5X      �� 5Y   
   -   5        $
 LblDate��5Y      $��تاریخ:��6!   $
��Left=�62   $��
71,115,0,0q�6M   $	��Top=�6a   $��30��6y   #���   6�   1   +Arash.PersianDateControls.PersianDatePicker  5Z   
   -   5        (  Name$( TxtDate��5Z   0   $
��Left=�6A   $��120,115,0,0q�6\   $	��Top=�6q   $��30��6�   $	��225��6�     PersianDatePickerStyle1#���  6�   $��2��6�   6�   5\      5]      