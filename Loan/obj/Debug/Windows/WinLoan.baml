   M S B A M L   `   `   `  ����U  QPersianDateControls, Version=1.2.4495.22344, Culture=neutral, PublicKeyToken=nullbDclr-namespace:Arash.PersianDateControls;assembly=PersianDateControlsArash.PersianDateControls  5   	    Loan   Loan.Windows.WinLoan   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   dpersianDateControlsDclr-namespace:Arash.PersianDateControls;assembly=PersianDateControls     5   	   -   5        $	��615��5   	   $	��960��6     � WindowStartupLocation$  CenterScreen=�6"    � WindowStyle$
 None=�6G   $��20(�6Z    � 
ResizeMode$ NoResize=�6h   $��	B Nazanin)�6~   ���	#FF192D6E$��2q�6�    � Triggers 5
      :� 5   
    :� RoutedEvent$ FrameworkElement.Loaded��6   .[�5      �� .��5      �� .��5      W�  �� TargetProperty$ Opacity�6&    W� From$ 0X�6J   	 W� To$ 1X�6S    g� Duration$ 0:0:.5D�6Z   	 g� FillBehavior$	 HoldEnd=�6l   5      5      5      .��5      � $��RightToLeft=�6   ���	#FFEEEEF2.+�5   
   �� -   5        ���	#FF192D6E$��0��5   '   ���	#FFEEEEF2$��30��6a   $	��Top=�6m   �� 5   
   -   5        
 ��Name$
 LblTitle��5      $��پرداخت وام��6"   $��22(�67   $
��Bold"�6E      LabelStyle1#���   6l   $��30��6�   $	��956��6�   $	��Top=�6�   $��Center=�6�   �� 5   
   -   5         ��Name$ BtnClose��5      $��×��6#   $��35��6/   $��30��6:   $��22(�6F   $��Arial)�6T   $
��Left=�6u   $	��Top=�6�     ButtonStyle2#���  6�   �� 5   
   -   5        $ BtnMinimize��5      $��-��6&   $��35,0,0,0q�62   $��35��6D   $��30��6O   $��27(�6[   $��Arial)�6i   $
��Left=�6�   $	��Top=�6�     ButtonStyle3#���  6�   �� 5   
   ���	#FF192D6E$��2q�6)   $��10,35,10,10q�6=    �� CornerRadius$ 10��6R   �� 5   
   $��30��6   $��	20,45,0,0q�6    ���	#FF192D6E$	��Top=�6F    �� RadiusY$ 7��6^    �� RadiusX$ 7��6j   $
��Left=�6v   $	��916��6�   �� 5   
   -   5        $
 LblId��5      $��شماره عضویت:��6   $��	20,45,0,0q�66   $��30��6I   $	��Top=�6U   $
��Left=�6m   #���   6�   �� 5   
   -   5        $
 LblPerId��5      $��
120,45,0,0q�6"   $��30��66   $	��Top=�6B   
��� � �$
��Bold"�6m   $
��Left=�6   #���   6�   $	��115��6�   �� 5   
   -   5        $
 LblName��5      $��نام:��6!   $��
235,45,0,0q�60   $��30��6D   $	��Top=�6P   $
��Left=�6h   #���   6�   �� 5    
   -	   5        $
 LblPerFirstName��5       $��
264,45,0,0q�6*   $��30��6>   $	��Top=�6J   $
��Left=�6b   
����  �$
��Bold"�6�   #���   6�   $	��206��6�   �� 5!   
   -
   5        $
 LblLastName��5!      $��نام خانوادگی:��6%   $��
470,45,0,0q�6=   $	��Top=�6Q   $��30��6i   $
��Left=�6u   #���   6�   �� 5"   
   -   5        $
 LblPerLastName��5"      $��
560,45,0,0q�6(   $��30��6<   $	��Top=�6H   $
��Left=�6`   
����  �$
��Bold"�6�   #���   6�   $	��202��6�   �� 5#   
   -   5        $
 LblNationalCode��5#      $��کد ملی:��6)   $��
762,45,0,0q�6;   $	��Top=�6O   $��30��6g   $
��Left=�6s   #���   6�   �� 5$   
   -   5        $
 LblPerNationalCode��5$      $��
821,45,0,0q�6,   $��30��6@   $	��Top=�6L   $
��Left=�6d   
����  �$
��Bold"�6�   #���   6�   $	��115��6�   �� 5&   
   $	��255��6   $��	20,80,0,0q�6"   $	��Top=�65   
������ ���	#FF192D6E$��2q�6~   $
��Left=�6�   $	��916��6�   .@�5'      �� -   5         ��Name$ TabLoan��5'      $��وام��6'     TabItemStyle1#���  64   
������ ���	#FF192D6E.��5(      � 
������ .+�5*      �� -   5        $
 LblLoanType��5*      $��نوع وام:��61   $
��Left=�6D   $��	51,10,0,0q�6_   $	��Top=�6r   $��30��6�   #���   6�   �� 5+      -   5        $
 LblLoanDate��5+      $��تاریخ پرداخت:��61   $
��Left=�6I   $��	10,80,0,0q�6d   $	��Top=�6w   $��30��6�   #���   6�   �� 5,      -   5        $
 LblLoanAmount��5,      $��مبلغ وام:��63   $
��Left=�6G   $��	43,45,0,0q�6b   $	��Top=�6u   $��30��6�   #���   6�   �� 5-      -   5        $
 LblInstallmentNum��5-      $��تعداد اقساط:��67   $
��Left=�6N   $��
685,45,0,0q�6i   $	��Top=�6}   $��30��6�   #���   6�   �� 5.      -   5        $ 
 LblInstallmentFirstPayDate��5.      $#��تاریخ اولین قسط:��6@   $
��Left=�6[   $��
344,80,0,0q�6v   $	��Top=�6�   $��30��6�   #���   6�   �� 5/      -   5        $
 LblInstallmentInterspace��5/      $#��فاصله بین اقساط:��6>   $
��Left=�6Y   $��
656,80,0,0q�6t   $	��Top=�6�   $��30��6�   #���   6�   �� 50      -   5        $
 LblLoanDescription��50      $��توضیحات:��68   $
��Left=�6K   $��
35,115,0,0q�6f   $	��Top=�6z   $��30��6�   #���   6�   �� 52      -   5         ��Name$ TxtLoanAmount��52      $��
111,45,0,0q�65    �� TextWrapping$
 Wrap=�6I     TextBoxStyle1#���  6]   $��3��6�    �� 	MaxLength$ 16��6�   $��30��6�   $	��Top=�6�   $
��Left=�6�   $	��200��6�   64  �� 53      -   5        $ TxtInstallmentNum��53      $
��Left=�69   $��30��6T   $��
777,45,0,0q�6`   $
 Wrap=�6t   $	��Top=�6�   $��90��6�   #���  6�   $��4��6�   $ 2��6�   6:  �� 54      -   5        $ TxtInstallmentInterspace��54      $
��Left=�6@   $��30��6[   $��
777,80,0,0q�6g   $
 Wrap=�6{   $	��Top=�6�   $��90��6�   #���  6�   $��6��6�   $ 2��6�   6  �� 55      -   5        $ TxtLoanDescription��55      $��111,115,0,0q�6:   $
 Wrap=�6O   #���  6c   $��7��6�   $��86��6�   $	��Top=�6�   $
��Left=�6�   $	��787��6�   1   +Arash.PersianDateControls.PersianDatePicker  57      -   5          Name$ TxtLoanDate��57   <   $
��Left=�6Q   $��
111,80,0,0q�6l   $	��Top=�6�   $��30��6�   $	��200��6�     PersianDatePickerStyle1#���  6�   $��2��6�   6    58      -   5        $  TxtInstallmentFirstPayDate��58   <   $
��Left=�6`   $��
464,80,0,0q�6{   $	��Top=�6�   $��30��6�   $	��187��6�   #���  6�   $��5��6�   6  �� 5:      -   5         ��Name$ CboLoanType��5:      $
��Left=�64   $��
111,10,0,0q�6O   $	��Top=�6c   $	��200��6{   $��30��6�   $��1��6�   .@�5;      �� $�� ��6'    �� 
IsSelected . 62   �� 5<      $��اقساطی��6'   �� 5=      $��مضاربه ای��6'   �� 5>      $��
ضروری��6'   5?      �� 5A      -   5        $ BtnUp��5A      $	��▲��6,   $��
847,45,0,0q�68   $��10(�6L   $��15��6Z   $	��Top=�6f   $
��Left=�6~   $��20��6�   6�   �� 5B      -   5        $ BtnDown��5B      $	��▼��6.   $��
847,60,0,0q�6:   $��10(�6N   $��15��6\   $	��Top=�6h   $
��Left=�6�   $��20��6�   6�   �� 5C      -   5        $ BtnUp1��5C      $	��▲��6-   $��
847,80,0,0q�69   $��10(�6M   $��15��6[   $	��Top=�6g   $
��Left=�6   $��20��6�   6�   �� 5D      -    5        $ BtnDown1��5D      $	��▼��6/   $��
847,95,0,0q�6;   $��10(�6O   $��15��6]   $	��Top=�6i   $
��Left=�6�   $��20��6�   6�   �� 5E      -!   5        $
 LblRial��5E      $��ریال��6-   $
��Left=�6<   $��
275,45,0,0q�6W   $	��Top=�6k   $��30��6�   #���   6�   �� 5F      -"   5        $
 LblMonth��5F      $��ماه��6.   $
��Left=�6<   $��
871,80,0,0q�6W   $	��Top=�6k   $��30��6�   #���   6�   �� 5G      -#   5        $
 LblLoanPenalty��5G      $,��&جریمه دیر کرد روزانه:��64   $
��Left=�6T   $��
316,45,0,0q�6o   $	��Top=�6�   $��30��6�   #���   6�   �� 5H      -$   5        $ TxtLoanPenalty��5H      $��
464,45,0,0q�66   $
 Wrap=�6J   #���  6^   $��3��6�   $ 16��6�   $��30��6�   $	��Top=�6�   $
��Left=�6�   $	��187��6�   {�.  6  �� 5I      -%   5        $
 	LblRial12��5I      $��ریال��6/   $
��Left=�6>   $��
615,45,0,0q�6Y   $	��Top=�6m   $��30��6�   #���   6�   5K      5L      �� 5M      -&   5        $��TabWage��5M      $��کارمزد��6%   #���  65   
������ ���	#FF192D6E.��5N      � 
������ .+�5P      �� -'   5        $
 LblWageType��5P      $��نوع کارمزد:��61   $
��Left=�6G   $��	26,10,0,0q�6b   $	��Top=�6u   $��30��6�   #���   6�   �� 5Q      -(   5        $
 LblWageCalculationType��5Q      $'��!نوع محاسبه کارمزد:��6<   $
��Left=�6Y   $��
358,10,0,0q�6t   $	��Top=�6�   $��30��6�   #���   6�   �� 5R      -)   5        $
 LblWagePercent��5R      $��	درصد:��64   $
��Left=�6D   $��
744,10,0,0q�6_   $	��Top=�6s   $��30��6�   #���   6�   $y�Hidden=�6�   �� 5S      -*   5        $
 LblWageAmount��5S      $��مبلغ کارمزد:��63   $
��Left=�6J   $��	18,45,0,0q�6e   $	��Top=�6x   $��30��6�   #���   6�   �� 5T      -+   5        $
 LblTotalDebt��5T      $��مجموع بدهی:��62   $
��Left=�6H   $��	10,80,0,0q�6c   $	��Top=�6v   $��30��6�   #���   6�   �� 5U      -,   5        $
 LblTotalPay��5U      $��خالص پرداختی:��61   $
��Left=�6I   $��
381,80,0,0q�6d   $	��Top=�6x   $��30��6�   #���   6�   �� 5V      --   5        $
 LblWageDescription��5V      $��توضیحات:��68   $
��Left=�6K   $��
33,115,0,0q�6f   $	��Top=�6z   $��30��6�   #���   6�   �� 5X      -.   5        $ TxtWagePercent��5X      $��30��66   $��
798,10,0,0q�6B   $
 Wrap=�6V   $	��Top=�6k   #���  6�   $��3��6�   $ 3��6�   $
��Left=�6<  $	��100��6W  $y�Hidden=�6c  �� 5Y      -/   5        $ TxtWageAmount��5Y      $��
109,45,0,0q�65   $
 Wrap=�6I   #���  6]   $��4��6�   $ 17��6�   $��30��6�   $	��Top=�6�   $
��Left=�6�   $	��224��6�   64  �� 5Z      -0   5        $ TxtTotalDebt��5Z      $��
109,80,0,0q�64   $
 Wrap=�6H   #���  6\   $��30��6�   $	��Top=�6�   $
��Left=�6�   $	��224��6�   {�.  6  �� 5[      -1   5        $ TxtTotalPay��5[      $��
493,80,0,0q�63   $
 Wrap=�6G   #���  6[   $��30��6�   $	��Top=�6�   $
��Left=�6�   $	��224��6�   {�.  6   �� 5\      -2   5        $ TxtWageDescription��5\      $��109,115,0,0q�6:   $
 Wrap=�6O   #���  6c   $��5��6�   $��86��6�   $	��Top=�6�   $
��Left=�6�   $	��789��6�   �� 5^      -3   5        $ CboWageType��5^      $
��Left=�64   $��
109,10,0,0q�6O   $	��Top=�6c   $	��224��6{   $��30��6�   $��1��6�   .@�5_      �� $�� ��6'    . 62   �� 5`      $��بدون کارمزد��6'   �� 5a      $!��کسر از مبلغ وام��6'   �� 5b      $��در اولین قسط��6'   �� 5c      $��طی تمام اقساط��6'   �� 5d      $��در آخرین قسط��6'   5e      �� 5f      -4   5        $ CboWageCalculationType��5f      $��
493,10,0,0q�6?   $	��Top=�6S   $��30��6k   $��2��6w   $
��Left=�6�   $	��224��6�   .@�5g      �� $�� ��6'    . 62   �� 5h      $��تعیین مبلغ��6'   �� 5i      $��
درصدی��6'   5j      �� 5l      -5   5        $ BtnUp2��5l      $	��▲��6-   $��
878,10,0,0q�69   $��10(�6M   $��15��6[   $	��Top=�6g   $
��Left=�6�   $��20��6�   $y�Hidden=�6�   �� 5m      -6   5        $ BtnDown2��5m      $	��▼��6/   $��
878,25,0,0q�6;   $��10(�6O   $��15��6]   $	��Top=�6i   $
��Left=�6�   $��20��6�   $y�Hidden=�6�   �� 5n      -7   5        $
 LblRial2��5n      $��ریال��6.   $
��Left=�6=   $��
297,45,0,0q�6X   $	��Top=�6l   $��30��6�   #���   6�   �� 5o      -8   5        $
 LblRial4��5o      $��ریال��6.   $��
297,80,0,0q�6=   $	��Top=�6Q   $��30��6i   $
��Left=�6u   #���   6�   �� 5p      -9   5        $
 LblRial5��5p      $��ریال��6.   $��
681,80,0,0q�6=   $	��Top=�6Q   $��30��6i   $
��Left=�6u   #���   6�   5r      5s      �� 5t      -:   5        $��TabInstallment��5t      $��
اقساط��6,   #���  6;   
������ ���	#FF192D6E.��5u      � 
������ .+�5w      �� ���	#FF192D6E$��2q�65   $
��Left=�6I   $	��101��6d   $��	10,10,0,0q�6q   $	��Top=�6�   $	��182��6�   $ 5��6�   �� 5y      ���	#FF192D6E$��2q�65   $��85��6I   $��10,116,380,0q�6U   $	��Top=�6k   $ 5��6�   &   System.Windows.Controls.DataGrid  5{      -;   5          Name$ DgdInstallment��5{      $��533,10,10,0q�67     DataGridStyle1#���  6L   ���	#FF192D6E$��2q�6�      AreRowDetailsFrozen .    
IsReadOnly .    GridLinesVisibility$ 
Horizontal=�6     HorizontalGridLinesBrush �	#FF192D6E   AlternatingRowBackground �	#FFF1F4F7 �� AlternationCount$ 2��6p     SelectionMode$ Single=�6�     AutoGenerateColumns .  -  'System.Windows.Input.KeyboardNavigation   TabNavigation$
 Once=�6�      CanUserSortColumns  .  $	��191��6�  $	��Top=�6	  ! %� IsVirtualizing! . " %� VirtualizationMode$" 	Recycling=�6N  ��6u   Z� #   Columns# 5|      0  *System.Windows.Controls.DataGridTextColumn  5}      ,  &System.Windows.Controls.DataGridColumn$   Header$$ تاریخ سررسید��61   %   Width5  /System.Windows.Controls.DataGridLengthConverter$	% 140 6G   1  +System.Windows.Controls.DataGridBoundColumn&   Binding& 6S   �� *	DueDate+'   ElementStyle' 5~   "   �� 5   &   ( �� 
TargetType"( ��6,   .�5�   *   �� ) �� Property) � ��* �� Value
* �OO/�6G   5�   '   5�        5�      $$ مبلغ قسط��61   $	% 170 6C   & 6O   �� *	DueAmount+' 5�   "   �� 5�   &   "( ��6,   .�5�   *   �� ) � ��
* �iii�6G   5�   '   5�      5�      �� 5�      -<   5        $
 LblInstallmentAmount��5�      $��مبلغ هر قسط:��6:   $
��Left=�6Q   $��
52,125,0,0q�6l   $	��Top=�6�   $��30��6�   #���   6�   �� 5�      -=   5        $ TxtInstallmentAmount��5�      $��153,125,0,0q�6<   $
 Wrap=�6Q   #���  6e   $ 17��6�   $��30��6�   $	��Top=�6�   $
��Left=�6�   $	��200��6�   {�.  6  �� 5�      ->   5        $
 LblRial1��5�      $��ریال��6.   $��317,125,0,0q�6=   $	��Top=�6R   $��30��6j   $
��Left=�6v   #���   6�   �� 5�      -?   5        $
 LblInstallmentWage��5�      $$��کارمزد در هر قسط:��68   $
��Left=�6T   $��
20,161,0,0q�6o   $	��Top=�6�   $��30��6�   #���   6�   �� 5�      -@   5        $
 
LblWageNum��5�      $��×  تعداد��60   $��353,161,0,0q�6C   $	��Top=�6X   $��30��6p   $
��Left=�6|   #���   6�   �� 5�      -A   5        $ TxtInstallmentWage��5�      $��153,161,0,0q�6:   $
 Wrap=�6O   #���  6c   $
��Left=�6�   $	��200��6�   {�.  $��30��6  $	��Top=�6  �� 5�      -B   5        $ 
TxtWageNum��5�      $��412,161,390,0q�62   $
 Wrap=�6I   #���  6]   $��30��6�   $	��Top=�6�   {�.  6�   �� 5�      -C   5        $
 LblRial3��5�      $��ریال��6.   $��317,161,0,0q�6=   $	��Top=�6R   $��30��6j   $
��Left=�6v   #���   6�   �� 5�      -D   5        $
 LblInsRound��5�      $��گرد کردن:��61   $
��Left=�6E   $��	20,20,0,0q�6`   $	��Top=�6s   $��30��6�   #���   6�   �� 5�      -E   5        $ TxtInsRound��5�      $
��Left=�63   $��30��6N   $��	99,20,0,0q�6Z   $
 Wrap=�6m   $	��Top=�6�   $��83��6�   #���  6�   $��4��6�   $ 1��6�   6-  �� 5�      -F   5        $ BtnUp3��5�      $	��▲��6-   $��
162,20,0,0q�69   $��10(�6M   $��15��6[   $	��Top=�6g   $
��Left=�6   $��20��6�   6�   �� 5�      -G   5        $ BtnDown3��5�      $	��▼��6/   $��
162,35,0,0q�6;   $��10(�6O   $��15��6]   $	��Top=�6i   $
��Left=�6�   $��20��6�   6�   �� 5�      -H   5        $ BtnCreatIns��5�      $��محاسبه اقساط��62   $��	20,55,0,0q�6I     ButtonStyle1#���  6\   $
��Left=�6�   $	��162��6�   $��8��6�   $��45��6�   $	��Top=�6�   6�   �� 5�      ���	#FF192D6E$��2q�65   $
��Left=�6I   $	��101��6d   $��
197,10,0,0q�6q   $	��Top=�6�   $	��331��6�   $ 5��6�   �� 5�      $��
208,15,0,0q�6    $
��Wrap=�64   + �� TextAlignment$+ Justify=�6H   $����در صورتی که جدول مقابل خالی بود یا میزان گرد کردن و یا تعداد اقساط  را کاهش دهید.
 اگر همچنان خالی بود مبلغ وام را افزایش دهید.��6`   $��91��6�   $	��Top=�6�   $
��Left=�6  $	��310��6+  5�      5�      �� 5�      -I   5        $��TabGuarantor��5�      $��    ضامن اول��6*   #���  6@   
������ ���	#FF192D6E.��5�      � 
������ .+�5�      �� $��30��6    $��151,10,151,0q�6,   ���	#FF192D6E$	��Top=�6U   ���	#FF879DE16m   �� 5�      -J   5        $
 
LblGuaName��5�      $��نام:��60   $��
151,10,0,0q�6?   $��30��6S   $	��Top=�6_   $
��Left=�6w   #���   6�   �� 5�      -K   5        $
 LblGuaPerFirstName��5�      $��
180,10,0,0q�69   $��30��6M   $	��Top=�6Y   $
��Left=�6q   
����  �$
��Bold"�6�   #���   6�   $	��156��6�   �� 5�      -L   5        $
 LblGuaLastName��5�      $��نام خانوادگی:��64   $��
336,10,0,0q�6L   $	��Top=�6`   $��30��6x   $
��Left=�6�   #���   6�   �� 5�      -M   5        $
 LblGuaPerLastName��5�      $��
426,10,0,0q�67   $��30��6K   $	��Top=�6W   $
��Left=�6o   
����  �$
��Bold"�6�   #���   6�   $	��157��6�   �� 5�      -N   5        $
 LblGuaNationalCode��5�      $��کد ملی:��68   $��
583,10,0,0q�6J   $	��Top=�6^   $��30��6v   $
��Left=�6�   #���   6�   �� 5�      -O   5        $
 LblGuaPerNationalCode��5�      $��
642,10,0,0q�6;   $��30��6O   $	��Top=�6[   $
��Left=�6s   
����  �$
��Bold"�6�   #���   6�   $	��115��6�   �� 5�      -P   5        $
 LblGuaBlockType��5�      $��مسدود سازی:��65   $
��Left=�6K   $��	10,45,0,0q�6f   $	��Top=�6y   $��30��6�   #���   6�   �� 5�      -Q   5        $
 LblGuaBlockAmount��5�      $��	مبلغ:��67   $
��Left=�6G   $��
327,45,0,0q�6b   $	��Top=�6v   $��30��6�   #���   6�   $y�Hidden=�6�   �� 5�      -R   5        $
 
LblGuaType��5�      $��نوع ضمانت:��60   $
��Left=�6E   $��	23,80,0,0q�6`   $	��Top=�6s   $��30��6�   #���   6�   �� 5�      -S   5        $
 LblGuaAmount��5�      $��	مبلغ:��62   $
��Left=�6B   $��
327,80,0,0q�6]   $	��Top=�6q   $��30��6�   #���   6�   �� 5�      -T   5        $
 LblGuaReceiptNumber��5�      $��شماره سند:��69   $
��Left=�6N   $��
599,80,0,0q�6i   $	��Top=�6}   $��30��6�   #���   6�   �� 5�      -U   5        $
 LblGuaDescription��5�      $��توضیحات:��67   $
��Left=�6J   $��
33,115,0,0q�6e   $	��Top=�6y   $��30��6�   #���   6�   �� 5�      -V   5        $ TxtGuaBlockAmount��5�      $��30��69   $��
373,45,0,0q�6E   $
 Wrap=�6Y   $	��Top=�6n   #���  6�   $��4��6�   $
��Left=�6�   $	��221��6�   $ 17��6  $y�Hidden=�6j  �� 5�      -W   5        $ TxtGuaAmount��5�      $��30��64   $��
373,80,0,0q�6@   $
 Wrap=�6T   $	��Top=�6i   #���  6�   $��4��6�   $
��Left=�6�   $	��221��6�   $ 17��6�   62  �� 5�      -X   5        $ TxtGuaReceiptNumber��5�      $��30��6;   $��
684,80,0,0q�6G   $
 Wrap=�6[   $	��Top=�6p   #���  6�   $��5��6�   $ 15��6�   $
��Left=�6�   $	��214��6�   �� 5�      -Y   5        $ TxtGuaDescription��5�      $��109,115,0,0q�69   $
 Wrap=�6N   #���  6b   $��6��6�   $��86��6�   $	��Top=�6�   $
��Left=�6�   $	��789��6�   �� 5�      -Z   5        $ CboGuaBlockType��5�      $
��Left=�68   $��
109,45,0,0q�6S   $	��Top=�6g   $	��213��6   $��30��6�   $��3��6�   .@�5�      �� $ ��عدم مسدود سازی��6'    . 6@   �� 5�      $��تا تسویه وام��6'   �� 5�      $%��تا مبلغ تعیین شده��6'   5�      �� 5�      -[   5        $ 
CboGuaType��5�      $
��Left=�63   $��
109,80,0,0q�6N   $	��Top=�6b   $	��213��6z   $��30��6�   $��3��6�   6�   �� 5�      -\   5        $ BtnGuaSelect��5�      $��انتخاب ضامن��63   $��	10,10,0,0q�6I   #���  6\   $
��Left=�6�   $	��136��6�   $��1��6�   $��30��6�   $	��Top=�6�   6�   �� 5�      -]   5        $ 	BtnGuaAdd��5�      $��ثبت جدید��60   $��
762,10,0,0q�6C   #���  6W   $
��Left=�6~   $	��136��6�   $��2��6�   $��30��6�   $	��Top=�6�   6�   �� 5�      -^   5        $
 LblRial6��5�      $��ریال��6.   $��
558,45,0,0q�6=   $	��Top=�6Q   $��30��6i   $
��Left=�6u   #���   6�   $y�Hidden=�6�   �� 5�      -_   5        $
 LblRial7��5�      $��ریال��6.   $��
558,80,0,0q�6=   $	��Top=�6Q   $��30��6i   $
��Left=�6u   #���   6�   5�      5�      �� 5�      -`   5        $��TabGuarantor2��5�      $��    ضامن دوم��6+   #���  6A   
������ ���	#FF192D6E.��5�      � 
������ .+�5�      �� $��30��6    $��151,10,151,0q�6,   ���	#FF192D6E$	��Top=�6U   ���	#FF879DE16m   �� 5�      -a   5        $
 LblGuaName2��5�      $��نام:��61   $��
151,10,0,0q�6@   $��30��6T   $	��Top=�6`   $
��Left=�6x   #���   6�   �� 5�      -b   5        $
 LblGuaPerFirstName2��5�      $��
180,10,0,0q�6:   $��30��6N   $	��Top=�6Z   $
��Left=�6r   
����  �$
��Bold"�6�   #���   6�   $	��156��6�   �� 5�      -c   5        $
 LblGuaLastName2��5�      $��نام خانوادگی:��65   $��
336,10,0,0q�6M   $	��Top=�6a   $��30��6y   $
��Left=�6�   #���   6�   �� 5�      -d   5        $
 LblGuaPerLastName2��5�      $��
426,10,0,0q�68   $��30��6L   $	��Top=�6X   $
��Left=�6p   
����  �$
��Bold"�6�   #���   6�   $	��157��6�   �� 5�      -e   5        $
 LblGuaNationalCode2��5�      $��کد ملی:��69   $��
583,10,0,0q�6K   $	��Top=�6_   $��30��6w   $
��Left=�6�   #���   6�   �� 5�      -f   5        $
 LblGuaPerNationalCode2��5�      $��
642,10,0,0q�6<   $��30��6P   $	��Top=�6\   $
��Left=�6t   
����  �$
��Bold"�6�   #���   6�   $	��115��6�   �� 5�      -g   5        $
 LblGuaBlockType2��5�      $��مسدود سازی:��66   $
��Left=�6L   $��	10,45,0,0q�6g   $	��Top=�6z   $��30��6�   #���   6�   �� 5�      -h   5        $
 LblGuaBlockAmount2��5�      $��	مبلغ:��68   $
��Left=�6H   $��
327,45,0,0q�6c   $	��Top=�6w   $��30��6�   #���   6�   $y�Hidden=�6�   �� 5�      -i   5        $
 LblGuaType2��5�      $��نوع ضمانت:��61   $
��Left=�6F   $��	23,80,0,0q�6a   $	��Top=�6t   $��30��6�   #���   6�   �� 5�      -j   5        $
 LblGuaAmount2��5�      $��	مبلغ:��63   $
��Left=�6C   $��
327,80,0,0q�6^   $	��Top=�6r   $��30��6�   #���   6�   �� 5�      -k   5        $
 LblGuaReceiptNumber2��5�      $��شماره سند:��6:   $
��Left=�6O   $��
599,80,0,0q�6j   $	��Top=�6~   $��30��6�   #���   6�   �� 5�      -l   5        $
 LblGuaDescription2��5�      $��توضیحات:��68   $
��Left=�6K   $��
33,115,0,0q�6f   $	��Top=�6z   $��30��6�   #���   6�   �� 5�      -m   5        $ TxtGuaBlockAmount2��5�      $��30��6:   $��
373,45,0,0q�6F   $
 Wrap=�6Z   $	��Top=�6o   #���  6�   $��4��6�   $
��Left=�6�   $	��221��6�   $ 17��6  $y�Hidden=�6l  �� 5�      -n   5        $ TxtGuaAmount2��5�      $��30��65   $��
373,80,0,0q�6A   $
 Wrap=�6U   $	��Top=�6j   #���  6�   $��4��6�   $
��Left=�6�   $	��221��6�   $ 17��6�   64  �� 5�      -o   5        $ TxtGuaReceiptNumber2��5�      $��30��6<   $��
684,80,0,0q�6H   $
 Wrap=�6\   $	��Top=�6q   #���  6�   $��5��6�   $ 15��6�   $
��Left=�6�   $	��214��6�   �� 5�      -p   5        $ TxtGuaDescription2��5�      $��109,115,0,0q�6:   $
 Wrap=�6O   #���  6c   $��6��6�   $
��Left=�6�   $	��789��6�   $��86��6�   $	��Top=�6�   �� 5�      -q   5        $ CboGuaBlockType2��5�      $
��Left=�69   $��
109,45,0,0q�6T   $	��Top=�6h   $	��213��6�   $��30��6�   $��3��6�   .@�5�      �� $ ��عدم مسدود سازی��6'    . 6@   �� 5�      $��تا تسویه وام��6'   �� 5�      $G��Aتا مبلغ تعیین شده (باقی مانده از وام)��6'   5�      �� 5�      -r   5        $ CboGuaType2��5�      $
��Left=�64   $��
109,80,0,0q�6O   $	��Top=�6c   $	��213��6{   $��30��6�   $��3��6�   6�   �� 5�      -s   5        $ BtnGuaSelect2��5�      $��انتخاب ضامن��64   $��	10,10,0,0q�6J   #���  6]   $
��Left=�6�   $	��136��6�   $��1��6�   $��30��6�   $	��Top=�6�   6�   �� 5�      -t   5        $ 
BtnGuaAdd2��5�      $��ثبت جدید��61   $��
762,10,0,0q�6D   #���  6X   $
��Left=�6   $	��136��6�   $��2��6�   $��30��6�   $	��Top=�6�   6�   �� 5�      -u   5        $
 LblRial8��5�      $��ریال��6.   $��
558,45,0,0q�6=   $	��Top=�6Q   $��30��6i   $
��Left=�6u   #���   6�   $y�Hidden=�6�   �� 5�      -v   5        $
 LblRial9��5�      $��ریال��6.   $��
558,80,0,0q�6=   $	��Top=�6Q   $��30��6i   $
��Left=�6u   #���   6�   5�      5�      �� 5�      -w   5        $��TabGuarantor3��5�      $��    ضامن سوم��6+   #���  6A   
������ ���	#FF192D6E.��5�      � 
������ .+�5�      �� $��30��6    $��151,10,151,0q�6,   ���	#FF192D6E$	��Top=�6U   ���	#FF879DE16m   �� 5�      -x   5        $
 LblGuaName3��5�      $��نام:��61   $��
151,10,0,0q�6@   $��30��6T   $	��Top=�6`   $
��Left=�6x   #���   6�   �� 5�      -y   5        $
 LblGuaPerFirstName3��5�      $��
180,10,0,0q�6:   $��30��6N   $	��Top=�6Z   $
��Left=�6r   
����  �$
��Bold"�6�   #���   6�   $	��156��6�   �� 5�      -z   5        $
 LblGuaLastName3��5�      $��نام خانوادگی:��65   $��
336,10,0,0q�6M   $	��Top=�6a   $��30��6y   $
��Left=�6�   #���   6�   �� 5�      -{   5        $
 LblGuaPerLastName3��5�      $��
426,10,0,0q�68   $��30��6L   $	��Top=�6X   $
��Left=�6p   
����  �$
��Bold"�6�   #���   6�   $	��157��6�   �� 5�      -|   5        $
 LblGuaNationalCode3��5�      $��کد ملی:��69   $��
583,10,0,0q�6K   $	��Top=�6_   $��30��6w   $
��Left=�6�   #���   6�   �� 5�      -}   5        $
 LblGuaPerNationalCode3��5�      $��
642,10,0,0q�6<   $��30��6P   $	��Top=�6\   $
��Left=�6t   
����  �$
��Bold"�6�   #���   6�   $	��115��6�   �� 5�      -~   5        $
 LblGuaBlockType3��5�      $��مسدود سازی:��66   $
��Left=�6L   $��	10,45,0,0q�6g   $	��Top=�6z   $��30��6�   #���   6�   �� 5�      -   5        $
 LblGuaBlockAmount3��5�      $��	مبلغ:��68   $
��Left=�6H   $��
327,45,0,0q�6c   $	��Top=�6w   $��30��6�   #���   6�   $y�Hidden=�6�   �� 5�      -�   5        $
 LblGuaType3��5�      $��نوع ضمانت:��61   $
��Left=�6F   $��	23,80,0,0q�6a   $	��Top=�6t   $��30��6�   #���   6�   �� 5�      -�   5        $
 LblGuaAmount3��5�      $��	مبلغ:��63   $
��Left=�6C   $��
327,80,0,0q�6^   $	��Top=�6r   $��30��6�   #���   6�   �� 5�      -�   5        $
 LblGuaReceiptNumber3��5�      $��شماره سند:��6:   $
��Left=�6O   $��
599,80,0,0q�6j   $	��Top=�6~   $��30��6�   #���   6�   �� 5�      -�   5        $
 LblGuaDescription3��5�      $��توضیحات:��68   $
��Left=�6K   $��
33,115,0,0q�6f   $	��Top=�6z   $��30��6�   #���   6�   �� 5�      -�   5        $ TxtGuaBlockAmount3��5�      $��30��6:   $��
373,45,0,0q�6F   $
 Wrap=�6Z   $	��Top=�6o   #���  6�   $��4��6�   $
��Left=�6�   $	��221��6�   $ 17��6  $y�Hidden=�6l  �� 5�      -�   5        $ TxtGuaAmount3��5�      $��30��65   $��
373,80,0,0q�6A   $
 Wrap=�6U   $	��Top=�6j   #���  6�   $��4��6�   $
��Left=�6�   $	��221��6�   $ 17��6�   64  �� 5�      -�   5        $ TxtGuaReceiptNumber3��5�      $��30��6<   $��
684,80,0,0q�6H   $
 Wrap=�6\   $	��Top=�6q   #���  6�   $��5��6�   $ 15��6�   $
��Left=�6�   $	��214��6�   �� 5�      -�   5        $ TxtGuaDescription3��5�      $��109,115,0,0q�6:   $
 Wrap=�6O   #���  6c   $��6��6�   $
��Left=�6�   $	��789��6�   $��86��6�   $	��Top=�6�   �� 5�      -�   5        $ CboGuaBlockType3��5�      $
��Left=�69   $��
109,45,0,0q�6T   $	��Top=�6h   $	��213��6�   $��30��6�   $��3��6�   .@�5�      �� $ ��عدم مسدود سازی��6'    . 6@   �� 5�      $��تا تسویه وام��6'   �� 5�      $G��Aتا مبلغ تعیین شده (باقی مانده از وام)��6'   5�      �� 5�      -�   5        $ CboGuaType3��5�      $
��Left=�64   $��
109,80,0,0q�6O   $	��Top=�6c   $	��213��6{   $��30��6�   $��3��6�   6�   �� 5�      -�   5        $ BtnGuaSelect3��5�      $��انتخاب ضامن��64   $��	10,10,0,0q�6J   #���  6]   $
��Left=�6�   $	��136��6�   $��1��6�   $��30��6�   $	��Top=�6�   6�   �� 5�      -�   5        $ 
BtnGuaAdd3��5�      $��ثبت جدید��61   $��
762,10,0,0q�6D   #���  6X   $
��Left=�6   $	��136��6�   $��2��6�   $��30��6�   $	��Top=�6�   6�   �� 5�      -�   5        $
 	LblRial10��5�      $��ریال��6/   $��
558,45,0,0q�6>   $	��Top=�6R   $��30��6j   $
��Left=�6v   #���   6�   $y�Hidden=�6�   �� 5�      -�   5        $
 	LblRial11��5�      $��ریال��6/   $��
558,80,0,0q�6>   $	��Top=�6R   $��30��6j   $
��Left=�6v   #���   6�   5�      5�      �� 5      -�   5        $��TabIntroducer��5      $��    معرف��6+   #���  6=   
������ ���	#FF192D6E.��5     � 
������ .+�5     �� -�   5        $��GrpReal��5     $��    حقیقی��6.   $��	10,5,10,0q�6A   $��70��6T   $	��Top=�6`   .��5     � .+�5     �� $��30��6(   $��135,0,135,-2q�64   ���	#FF192D6E$	��Top=�6]   ���	#FF879DE16u   �� 5     -�   5        $
 LblRecomName��5  $   $��نام:��6:   $��
135,0,0,-2q�6I   $��30��6]   $	��Top=�6i   $
��Left=�6�   #���   6�   �� 5     -�   5        $
 LblRecomPerFirstName��5  $   $��
164,0,0,-2q�6C   $��30��6W   $	��Top=�6c   $
��Left=�6{   
����  �$
��Bold"�6�   #���   6�   $	��157��6�   �� 5     -�   5        $
 LblRecomLastName��5  $   $��نام خانوادگی:��6>   $��
321,0,0,-2q�6V   $	��Top=�6j   $��30��6�   $
��Left=�6�   #���   6�   �� 5	     -�   5        $
 LblRecomPerLastName��5	  $   $��
411,0,0,-2q�6A   $��30��6U   $	��Top=�6a   $
��Left=�6y   
����  �$
��Bold"�6�   #���   6�   $	��156��6�   �� 5
     -�   5        $
 LblRecomNationalCode��5
  $   $��کد ملی:��6B   $��
567,0,0,-2q�6T   $	��Top=�6h   $��30��6�   $
��Left=�6�   #���   6�   �� 5     -�   5        $
 LblRecomPerNationalCode��5  $   $��
626,0,0,-2q�6E   $��30��6Y   $	��Top=�6e   $
��Left=�6}   
����  �$
��Bold"�6�   #���   6�   $	��115��6�   �� 5     -�   5        $ BtnRecomSelect��5  %   $��انتخاب معرف��6=   $��10,0,0,0q�6S   #���  6e   $��1��6�   $��30��6�   $	��Top=�6�   $
��Left=�6�   $	��120��6�   6�   �� 5     -�   5        $ BtnRecomAdd��5  %   $��ثبت جدید��6:   $��
746,0,0,-2q�6M   #���  6a   $��2��6�   $
��Left=�6�   $	��120��6�   $��30��6�   $	��Top=�6�   6�   5     5     �� 5     -�   5        $��GrpLegal��5     $��    حقوقی��6/   $��
10,80,10,0q�6B   $��70��6V   $	��Top=�6b   .��5     � .+�5     �� -�   5        $
 LblRecomType��5  $   $��انتخاب:��6:   $
��Left=�6L   $��10,0,0,0q�6g   #���   6y   $��30��6�   $	��Top=�6�   �� 5     -�   5        $ CboRecom��5  '   , �� 
IsEditable, . $��70,0,0,0q�6L   $	��Top=�6^   $��30��6v   $��3��6�   $
��Left=�6�   $	��796��6�   5     5     	� 5     -�   5        $��RdoReal��5  "   - d� 	IsChecked$
- TrueY�61   $
��Left=�6B   $��	20,13,0,0q�6]   $	��Top=�6p   6�   	� 5     -�   5        $��RdoLegal��5  "   $
��Left=�62   $��	20,93,0,0q�6M   $	��Top=�6`   6x   �� 5     -�   5        $
 LblRecomDescription��5     $��توضیحات:��69   $
��Left=�6L   $��
10,155,0,0q�6g   $	��Top=�6{   $��30��6�   #���   6�   �� 5     -�   5        $ TxtRecomDescription��5     $��86,155,10,0q�6;   $
 Wrap=�6P   #���  6d   $��5��6�   $��46��6�   $	��Top=�6�   5     5      5!       5#  
   -�   5        $ DgdLoan��5#     $��
20,340,0,0q�6$   #���  68   ���	#FF192D6E$��2q�6�    .  . $ 
Horizontal=�6�    �	#FF192D6E �	#FFF1F4F7$ 2��6`  $ Single=�6u   .  $
 Once=�6�    .  $	��216��6�  $	��Top=�6�  $
��Left=�6  $	��916��6,  ��6a   Z� # 5$       5&     $$ شماره وام��6%   $% 70 68   & 6C   �� *Id+  5(     $$ تاریخ پرداخت��6%   $	% 100 6;   & 6G   �� *
LoanDate+  5*     $$ نوع وام��6%   $	% 120 66   & 6B   �� *
LoanType+  5,     $$ مبلغ وام��6%   $	% 130 67   & 6C   �� *
LoanAmount+. �� StringFormat$. {0:N0}��  5.     $$ نوع کارمزد��6%   $	% 110 69   & 6E   �� *
WageType+  50     $$ مبلغ کارمزد��6%   $	% 130 6:   & 6F   �� *
WageAmount+$. {0:N0}��  52     $$ تعداد اقساط��6%   $% 85 6:   & 6E   �� *LoanInstallmentNum+  54     $$ توضیحات��6%   $	% 120 66   & 6B   �� *LoanDescription+57     �� 59  
   -�   5        $ BtnAdd��59     $��ثبت��6!   $��293,561,0,0q�6/   #���  6D   $
��Left=�6k   $	��120��6�   $��8��6�   / �� 	IsDefault/ . $��30��6�   $	��Top=�6�   �� 5:  
   -�   5        $ BtnNew��5:     $��جدید��6!   $��543,561,293,0q�60   #���  6G   $��9��6n   $��30��6�   $	��Top=�6�   �� 5;  
   -�   5        $ 	BtnDelete��5;     $��حذف��6$   $��418,561,0,0q�62   #���  6G   $��9��6o   $��30��6|   $	��Top=�6�   $
��Left=�6�   $	��120��6�   6�   �� 5=  
   -�   5        $��ChkGuarantor��5=     $
��Left=�6'   $��
157,91,0,0q�6B   $	��Top=�6V   $
- TrueY�6n   $��LeftToRight=�6   6�   �� 5>  
   -�   5        $��ChkGuarantor2��5>     $
��Left=�6(   $��
248,91,0,0q�6C   $	��Top=�6W   $
- TrueY�6o   $��LeftToRight=�6�   6�   �� 5?  
   -�   5        $��ChkGuarantor3��5?     $
��Left=�6(   $��
341,91,0,0q�6C   $	��Top=�6W   $
- TrueY�6o   $��LeftToRight=�6�   6�   �� 5@  
   -�   5        $��ChkIntroducer��5@     $
��Left=�6(   $��
438,91,0,0q�6C   $	��Top=�6W   $
- TrueY�6o   $��LeftToRight=�6�   6�   5B     5C     