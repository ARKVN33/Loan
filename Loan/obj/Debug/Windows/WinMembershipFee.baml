   M S B A M L   `   `   `  ����  Loan#    Loan.Windows.WinMembershipFee   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -   5        $	��135��5   	   $	��428��6     � WindowStartupLocation$  CenterScreen=�6"    � WindowStyle$
 None=�6G   $��20(�6Z    � 
ResizeMode$ NoResize=�6h   $��	B Nazanin)�6~   ���	#FF192D6E$��2q�6�    � Triggers 5	      :� 5
   
    :� RoutedEvent$ Window.Loaded��6   .[�5      �� .��5      �� .��5      W�  �� TargetProperty$ Opacity�6&    W� From$ 0X�6J   	 W� To$ 1X�6S    g� Duration$ 0:0:.5D�6Z   	 g� FillBehavior$	 HoldEnd=�6l   5      5      5      .��5      � $��RightToLeft=�6   ���	#FFEEEEF2.+�5   
   �� -   5        ���	#FF192D6E$��0��5   '   ���	#FFEEEEF2$��30��6a   $	��Top=�6m   �� 5   
   -   5        
 ��Name$
 LblTitle��5      $%��ثبت مبلغ حق عضویت��6"   $��22(�6>   $
��Bold"�6L   $��0q�6^   $	��Top=�6i      LabelStyle1#���   6�   $��30��6�   $��Center=�6�   �� 5   
   -   5         ��Name$ BtnClose��5      $��×��6#   $��35��6/   $��30��6:   $��22(�6F   $��Arial)�6T   $
��Left=�6u   $	��Top=�6�     ButtonStyle2#���  6�   �� 5   
   -   5        $ BtnMinimize��5      $��-��6&   $��35,0,0,0q�62   $��35��6D   $��30��6O   $��27(�6[   $��Arial)�6i   $
��Left=�6�   $	��Top=�6�     ButtonStyle3#���  6�   �� 5   
   ���	#FF192D6E$��2q�6)   $��10,35,10,10q�6=    �� CornerRadius$ 10��6R   �� 5   
   $��حق عضویت:��6   $��	20,45,0,0q�6$   $	��Top=�67   $
��Left=�6O   $��30��6j   #���   6v   �� 5   
   -   5         ��Name$ TxtFee��5      $��30��6"   $��
111,45,0,0q�6.    �� TextWrapping$
 Wrap=�6B   $	��Top=�6W     TextBoxStyle1#���  6o   $��3��6�   $
��Left=�6�   $	��293��6�    �� 	MaxLength$ 17��6�   6  �� 5   
   -   5        $ BtnAdd��5      $��ثبت ��6!   $��
111,80,0,0q�60     ButtonStyle1#���  6D   $��1��6k    �� 	IsDefault . $��30��6�   $	��Top=�6�   $
��Left=�6�   $	��144��6�   6�   �� 5   
   -   5        $ BtnNew��5      $��جدید��6!   $��
260,80,0,0q�60   #���  6D   $��1��6k   $��30��6x   $	��Top=�6�   $
��Left=�6�   $	��144��6�   �� 5    
   -	   5        $
 LblRial3��5       $��ریال��6"   $
��Left=�61   $��
368,45,0,0q�6L   $	��Top=�6`   $��30��6x   #���   6�   5"      5#      