   M S B A M L   `   `   `  ����  Loan    Loan.Windows.WinLoanFund   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -   5        $	��515��5   	   $	��700��6     � WindowStartupLocation$  CenterScreen=�6"    � WindowStyle$
 None=�6G   
������ $��20(�6s    � 
ResizeMode$ NoResize=�6�   $��	B Nazanin)�6�   ���	#FF192D6E$��2q�6�    � Triggers 5	      :� 5
   
    :� RoutedEvent$ FrameworkElement.Loaded��6   .[�5      �� .��5      �� .��5      W�  �� TargetProperty$ Opacity�6&    W� From$ 0X�6J   	 W� To$ 1X�6S    g� Duration$ 0:0:.5D�6Z   	 g� FillBehavior$	 HoldEnd=�6l   5      5      5      .��5      � $��RightToLeft=�6   ���	#FFEEEEF2.+�5   
   �� -   5        ���	#FF192D6E$��0��5   '   ���	#FFEEEEF2$��30��6a   $	��Top=�6m   �� 5   
   -   5        
 ��Name$
 LblTitle��5      $$��ثبت مشخصات صندوق��6"   $��22(�6=   $
��Bold"�6K   $	��Top=�6]      LabelStyle1#���   6�   $��30��6�   $	��696��6�   $��Center=�6�   �� 5   
   -   5         ��Name$ BtnClose��5      $��×��6#   $��35��6/   $��30��6:   $��22(�6F   $��Arial)�6T   $
��Left=�6u   $	��Top=�6�     ButtonStyle2#���  6�   �� 5   
   -   5        $ BtnMinimize��5      $��-��6&   $��35,0,0,0q�62   $��35��6D   $��30��6O   $��27(�6[   $��Arial)�6i   $
��Left=�6�   $	��Top=�6�     ButtonStyle3#���  6�   �� 5   
   ���	#FF192D6E$��2q�6)   $��10,35,10,10q�6=    �� CornerRadius$ 10��6R   �� 5   
   -   5         ��Name$ GroupBox��5      $��اطلاعات صندوق��6%   $
��Bold"�6<   $��	20,45,0,0q�6N   $��RightToLeft=�6a   $	��300��6}   $	��Top=�6�   ���	#FF192D6E$
��Left=�6�   $	��656��6�   �� 5   
   -   5        $ 	GroupBox1��5      $(��"اطلاعات مالی صندوق��6&   $
��Bold"�6B   $��
20,350,0,0q�6T   $��RightToLeft=�6h   $	��106��6�   $	��Top=�6�   ���	#FF192D6E J� RenderTransformOrigin$ 0.5,0.5&�6�   $
��Left=�6�   $	��656��6�   �� 5   
   -   5        $ 	GroupBox3��5      $��
تصویر��6&   $��
501,60,0,0q�65   $��RightToLeft=�6I   ���	#FF192D6E$	��220��6}   $	��Top=�6�   $
��Left=�6�   $	��165��6�   �� 5   
   -	   5        $
 LblName��5      $��نام صندوق:��6!   $
��Left=�66   $��	68,75,0,0q�6Q   $	��Top=�6d   $��30��6|   #���   6�   �� 5   
   -
   5        $
 LblTell1��5      $!��شماره تماس اول:��6"   $
��Left=�6<   $��
32,110,0,0q�6W   $	��Top=�6k   $��30��6�   #���   6�   �� 5    
   -   5        $
 LblTell2��5       $!��شماره تماس دوم:��6"   $
��Left=�6<   $��
30,145,0,0q�6W   $	��Top=�6k   $��30��6�   #���   6�   �� 5!   
   -   5        $
 LblFax��5!      $��فکس:��6    $
��Left=�6/   $��101,180,0,0q�6J   $	��Top=�6_   $��30��6w   #���   6�   �� 5"   
   -   5        $
 LblEmail��5"      $��ایمیل:��6"   $
��Left=�63   $��
99,215,0,0q�6N   $	��Top=�6b   $��30��6z   #���   6�   �� 5#   
   -   5        $
 LblPostalCode��5#      $��کد پستی:��6'   $
��Left=�6:   $��
77,250,0,0q�6U   $	��Top=�6i   $��30��6�   #���   6�   �� 5$   
   -   5        $
 
LblAddress��5$      $��	آدرس:��6$   $
��Left=�64   $��
97,285,0,0q�6O   $	��Top=�6c   $��30��6{   #���   6�   �� 5%   
   -   5        $
 LblInitialBalance��5%      $��موجودی اولیه:��6+   $
��Left=�6C   $��
30,380,0,0q�6^   $	��Top=�6r   $��30��6�   #���   6�   �� 5&   
   -   5        $
 
LblPenalty��5&      $4��.مبلغ جریمه دیرکرد روزانه:��6$   $
��Left=�6H   $��315,380,0,0q�6c   $	��Top=�6x   $��30��6�   #���   6�   �� 5'   
   -   5        $
 LblWage��5'      $*��$درصد پیش فرض کارمزد:��6!   $
��Left=�6@   $��
30,415,0,0q�6[   $	��Top=�6o   $��30��6�   #���   6�   �� 5)   
   -   5         ��Name$ TxtName��5)      $��30��6#   $��
290,75,0,0q�6/    �� TextWrapping$
 Wrap=�6C   $	��Top=�6X     TextBoxStyle1#���  6p   $��1��6�    �� 	MaxLength$	 100��6�   $
��Left=�6�   $	��205��6�   �� 5*   
   -   5        $ TxtTell1��5*      $
��Left=�6$   $��30��6?   $��152,110,0,0q�6K   $
 Wrap=�6`   $	��Top=�6u   $	��344��6�   #���  6�   $��2��6�   $ 11��6�   $��LeftToRight=�6�   6  �� 5+   
   -   5        $ TxtTell2��5+      $
��Left=�6$   $��30��6?   $��152,145,0,0q�6K   $
 Wrap=�6`   $	��Top=�6u   $	��344��6�   #���  6�   $��3��6�   $ 11��6�   $��LeftToRight=�6�   6  �� 5,   
   -   5        $ TxtFax��5,      $
��Left=�6"   $��30��6=   $��152,180,0,0q�6I   $
 Wrap=�6^   $	��Top=�6s   $	��344��6�   #���  6�   $��4��6�   $ 11��6�   $��LeftToRight=�6�   6  �� 5-   
   -   5        $ TxtEmail��5-      $��30��6$   $��152,215,0,0q�60   $
 Wrap=�6E   $	��Top=�6Z   #���  6r   $��5��6�   $	 200��6�   $��LeftToRight=�6�   $
��Left=�6�   $	��344��6  65  �� 5.   
   -   5        $ TxtPostalCode��5.      $
��Left=�6)   $��30��6D   $��152,250,0,0q�6P   $
 Wrap=�6e   $	��Top=�6z   $	��344��6�   #���  6�   $��6��6�   $ 10��6�   $��LeftToRight=�6  6  �� 5/   
   -   5        $ 
TxtAddress��5/      $
��Left=�6&   $��50��6A   $��152,285,0,0q�6M   $
 Wrap=�6b   $	��Top=�6w   $	��514��6�   #���  6�   $��7��6�   �� 50   
   -   5        $ TxtInitialBalance��50      $��132,380,0,0q�6-   $
 Wrap=�6B   #���  6V   $��8��6~   $ 17��6�   $��30��6�   $	��Top=�6�   $
��Left=�6�   $	��178��6�   60  �� 51   
   -   5        $ 
TxtPenalty��51      $��488,380,0,0q�6&   $
 Wrap=�6;   #���  6O   $��9��6w   $ 17��6�   $��30��6�   $	��Top=�6�   $
��Left=�6�   $	��178��6�   6"  �� 52   
   -   5        $ TxtWage��52      $��198,415,0,0q�6#   $
 Wrap=�68   #���  6L   $��10��6t   $ 3��6�   $��30��6�   $	��Top=�6�   $
��Left=�6�   $	��112��6�   6  �� 54   
   -   5         ��Name$ ImgPersonnelImage��54      $��516,90,45,276q�6+   $��../Icon/Eye.png��6B   $��Center=�6[   �� 56   
   -   5        $ BtnChoosePicture��56      $$��انتخاب آرم صندوق��6+   $��516,240,0,0q�6F     ButtonStyle1#���  6[   $��11��6�   $
��Left=�6�   $	��135��6�   $��30��6�   $	��Top=�6�   �� 57   
   -   5        $ BtnAdd��57      $��ثبت��6!   $��281,461,280,0q�6/   #���  6F   $��11��6m   $��30��6�   $	��Top=�6�   �� 59   
   -    5        $
 LblRial��59      $��ریال��6!   $
��Left=�60   $��274,380,0,0q�6K   $	��Top=�6`   $��30��6x   #���   6�   �� 5:   
   -!   5        $
 LblRial2��5:      $��ریال��6"   $
��Left=�61   $��630,380,0,0q�6L   $	��Top=�6a   $��30��6y   #���   6�   �� 5;   
   -"   5        $
 LblSName��5;      $$��صندوق قرض الحسنه��6"   $
��Left=�6=   $��
152,75,0,0q�6X   $	��Top=�6l   $��30��6�   
����������	#FF192D6E$��1,1,0,1q�6�   #���   6�   5=      5>      