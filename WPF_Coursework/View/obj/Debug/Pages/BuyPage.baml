   M S B A M L   `   `   `  ����?  ;View, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:View.Pages
View.Pages  5          View.Pages.BuyPage   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      *localclr-namespace:View.Pages     5      A =Wpf.Ui, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null/ui&http://schemas.lepo.co/wpfui/2022/xaml  5        N� Title$  BuyPage��5	      $
��1100��5
      $	��650��5         ApplicationBackgroundBrush  Wpf.Ui.Markup.Design   
Background# �   5        TextFillColorPrimaryBrush   
Foreground# �  5       N� 
Foreground# �  5       � DataContext 5      ��  �� RelativeSource �� *Self+.��5      ��  �� Orientation$ Vertical=�6   .+�5   
     Wpf.Ui.Controls.TextBlock  $��Total Price��6   $��Center=�6*   $��0 30 0 0q�6G     5   
   $��Center=�63   $��0 10 0 0q�6P   $��20(�6b   ��6   �� *
TotalPrice+6b     5   
   $��Address��6   $��Center=�6&   $��0 30 0 0q�6C     5   
   $
��Area��6   $��Center=�6#   $��0 10 0 0q�6@   $��20(�6R   �� 5   
   -   5         ��Name$ AreasComboBox��5      $��0 4 0 0q�5      $	��500��6$    �� DisplayMemberPath$
 Name��5      	 �� SelectedValuePath$	 Id��5      ��5      �� *Areas+5        5"   
   $��
Settlement��6   $��Center=�6)   $��0 10 0 0q�6F   $��20(�6X   �� 5#   
   -   5        $ SettlementsComboBox��5$      $��0 4 0 0q�5#      $	��500��6$   $
 Name��5&      $	 Id��5'      ��5%      �� *Settlements+5(        5+   
   $��Post office��6   $��Center=�6*   $��0 10 0 0q�6G   $��20(�6Y   �� 5,   
   -   5        $ PostOfficesComboBox��5-      $��0 4 0 0q�5,      $	��500��6$   $
 Name��5/      $	 Id��50      ��5.      �� *PostOffices+50        Wpf.Ui.Controls.Button  53   
   -   5        $��Center=�53      $��0 30 0 0q�61   $	��500��6C   $��20(�6O   .��54      �� $ 
Horizontal=�6   .+�55         Wpf.Ui.Controls.SymbolIcon  
   Symbol$
 ShoppingBag24=�6    $��0 0 10 0q�67     56      $��Order��6   57      58      59      5:      