   M S B A M L   `   `   `  ����  Loan    Loan.Windows.WinLicense   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -   5        $	��370��5   	   $	��600��6     � WindowStartupLocation$  CenterScreen=�6"   $��20(�6G    � 
ResizeMode$ NoResize=�6U   $��	B Nazanin)�6k   ���	#FF192D6E$��2q�6�    � WindowStyle$
 None=�6�    � Triggers 5	      :� 5
   
    :� RoutedEvent$ FrameworkElement.Loaded��6   .[�5      �� .��5      �� .��5      W�  �� TargetProperty$ Opacity�6&    W� From$ 0X�6J   	 W� To$ 1X�6S    g� Duration$ 0:0:.5D�6Z   	 g� FillBehavior$	 HoldEnd=�6l   5      5      5      .��5      � $��RightToLeft=�6   ���	#FFEEEEF2.+�5   
   �� -   5        ���	#FF192D6E$��0��5   '   ���	#FFEEEEF2$��30��6a   $	��Top=�6m   �� 5   
   -   5        
 ��Name$
 LblTitle��5      $$��فعال سازی برنامه��6"   $��22(�6=   $
��Bold"�6K   $��0q�6]   $	��Top=�6h      LabelStyle1#���   6�   $��30��6�   $��Center=�6�   �� 5   
   -   5         ��Name$ BtnClose��5      $��×��6#   $��35��6/   $��30��6:   $��22(�6F   $��Arial)�6T   $
��Left=�6u   $	��Top=�6�     ButtonStyle2#���  6�   �� 5   
   -   5        $ BtnMinimize��5      $��-��6&   $��35,0,0,0q�62   $��35��6D   $��30��6O   $��27(�6[   $��Arial)�6i   $
��Left=�6�   $	��Top=�6�     ButtonStyle3#���  6�   �� 5   
   ���	#FF192D6E$��2q�6)   $��10,35,10,10q�6=    �� CornerRadius$ 10��6R   �� 5   
   ���	#FF192D6E$��1q�6)   $��
25,50,0,25q�6=   $ 5��6Q   $
��Left=�6b   $	��546��6}   �� 5   
   -   5         ��Name$ TxtSerialNum��5      $��30��6(   $��
40,190,0,0q�64    �� TextWrapping$
 Wrap=�6H   $	��Top=�6]     TextBoxStyle1#���  6u   $��1��6�    �� 	MaxLength$ 50��6�   $��Center=�6�    �� 
IsReadOnly . $
��Bold"�6�   $
��Left=�6  $	��516��6  �� 5   
   -   5        $ TxtLicensekey��5      $��
40,260,0,0q�6)   $
 Wrap=�6=   #���  6Q   $��1��6y   $ 50��6�   $��Center=�6�   $
��Bold"�6�   $
��Left=�6�   $	��516��6�   $��30��6�   $	��Top=�6�   �� 5    
   $��شماره سریال:��6   $
��Left=�6'   $��
40,155,0,0q�6B   $	��Top=�6V   $��30��6n   #���   6z   $
��Bold"�6�   �� 5!   
   $��کد فعال سازی:��6   $
��Left=�6(   $��
40,225,0,0q�6C   $	��Top=�6W   $��30��6o   #���   6{   $
��Bold"�6�   �� 5"   
   $$��فعال سازی آفلاین��6   $��195,50,194,0q�6+   $	��Top=�6A   $��36(�6Y   �� 5$   
   -   5        $ BtnRegistration��5$      $��فعال سازی��6*   $��
40,301,0,0q�6>     ButtonStyle1#���  6R   $
��Left=�6y   $	��516��6�    �� 	IsDefault . $��12��6�   $��30��6�   $	��Top=�6�   �� 5%   
   $���zلطفا جهت دریافت کد فعال سازی،کد 28 رقمی شماره سریال را ارسال فرمایید��6   $��57,120,56,0q�6_   $	��Top=�6t   $��30��6�   #���   6�   5'      5(      