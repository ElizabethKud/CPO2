   M S B A M L   `   `   `  ����  CPO2!    CPO2.Views.UsersAdminWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$7  1Управление пользователями��5   	   $	��400��6+   $	��600��68   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� -   5         ��Name$ UsersListView��5      ��6*   �� *Users+ �� View 5      �� 5      .P�5      �� $��
Логин��6%    �� Width$	 150��6^    �� DisplayMemberBinding 64   �� *
Username+6^   �� 5      $��Роль��6%   $	 100��6Y    63   �� *Role+6Y   �� 5      $%��Дата регистрации��6%   $	 150��6q    6?   �� *RegistrationDate+6q   �� 5      $��Активен��6%    �� CellTemplate 5      �� 5      .R�5   "   ��  d� 	IsChecked 6+   �� *
IsActive+ �� Mode$ TwoWay=���5   +   �� *!DataContext.ToggleActiveCommand+	 �� RelativeSource	 �� 
 �� AncestorType

 Window��5   +   �� 5      5      5      5      �� 5    
   $��1��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   $��10q�6W   .+�5!      �� $��Удалить��6   $��5q�6M   ��6'   �� *DeleteUserCommand+6M   �� 5"      -   5        $��Закрыть��5"      $��5q�6;   5#      5$      5%      