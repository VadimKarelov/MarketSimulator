   M S B A M L   `   `   `  ���� J  FMarketSimulator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:MarketSimulatorMarketSimulator       MarketSimulator.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     /localclr-namespace:MarketSimulator     -     � Title$  Market Model��$	��700��$
��1300��$��20(����	#FFDCFFC8.��� .+��� -    ��Name$ image_SimulatorArea��$
��Left=�$	��400��$��0,0,0,0q�$	��Top=�$	��400�� J� RenderTransformOrigin$ -0.037,-0.048&�$��mandarin.jpg��$
��Fill=��� -    ��Name$ label_Average��$ ��Средняя цена: 0��$
��Left=�$��	0,420,0,0q�$	��Top=��� -   $ label_MaxPrice��$*��$Максимальная цена: 0��$
��Left=�$��440,420,0,0q�$	��Top=��� -   $ image_GraphicAverage��$
��Left=�$	��200��$��	0,460,0,0q�$	��Top=�$	��400���� -   $ image_GraphicMaxPrice��$
��Left=�$	��200��$��440,460,0,0q�$	��Top=�$	��400���� -   $ label_0��$��0��$
��Left=�$��410,620,0,0q�$	��Top=��� -   $ label_UpperBound��$	��100��$
��Left=�$��400,460,0,0q�$	��Top=��� -	   $ 	label_Day��$��День: 0��$
��Left=�$��
420,20,0,0q�$	��Top=��� -
    ��Name$ button_Pause��$��
Старт��$
��Left=�$��
560,25,0,0q�$	��Top=�$��75���� -   $ label_Sellers��$��Продавцы��$
��Left=�$��
420,70,0,0q�$	��Top=��� -   $ label_Clients��$��Покупатели��$
��Left=�$��420,100,0,0q�$	��Top=��� -   $ label_SellersCounter��$��1��$
��Left=�$��
550,70,0,0q�$	��Top=��� -   $ label_ClientsCounter��$��1��$
��Left=�$��550,100,0,0q�$	��Top=��� -    ��Name$ slider_Sellers��$
��Left=�$��
580,82,0,0q�$	��Top=� � Value$ 5Q� � Minimum$ 1Q�$	��100���� -   $ slider_Clients��$
��Left=�$��580,112,0,0q�$	��Top=�$ 5Q�$ 1Q�$	��100���� -   $ label_Warning��$/��)Симуляция остановлена��$
��Left=�$��
650,20,0,0q�$	��Top=����	#FFFFA200�� -   $ label_InfluenceWarning��$>��8Требуется остановка симуляции��$
��Left=�$��
910,20,0,0q�$	��Top=����	#FFFFA200$	��343���� -   $ label_ListOfSellers��$&�� Список продавцов:��$
��Left=�$��420,140,0,0q�$	��Top=�$��16(��� -   $ label_ListOfClients��$*��$Список покупателей:��$
��Left=�$��640,140,0,0q�$	��Top=�$��16(��� -   $ label_Speed��$��Скорость��$
��Left=�$��
730,70,0,0q�$	��Top=�$y�Hidden=��� -   $ slider_Speed��$
��Left=�$��730,112,0,0q�$	��Top=�$ 1Q�$ 1Q�$	��100��$y�Hidden=��� -   $ label_InfluenceOnSellers��$,��&Влияние на продавцов��$
��Left=�$��
910,60,0,0q�$	��Top=��� -   $ button_IncreasePrice��$!��Повышение цены��$
��Left=�$��910,100,0,0q�$	��Top=�
 � Tag$ si���� -   $ button_DecreasePrice��$��Снижение цены��$
��Left=�$��1100,100,0,0q�$	��Top=�$ sd���� -   $ label_InfluenceOnClients��$0��*Влияние на покупателей��$
��Left=�$��910,160,0,0q�$	��Top=��� -   $ button_IncreaseMaxPrice��$!��Повышение цены��$
��Left=�$��910,200,0,0q�$	��Top=�$ ci���� -   $ button_DecreaseMaxPrice��$��Снижение цены��$
��Left=�$��1100,200,0,0q�$	��Top=�$ cd���� $
��Left=�$	��212��$��
910,20,0,0q�
���   �$	��Top=�$	��343���� -   $ button_Help��$
��Left=�$��1039,390,0,0q�$	��Top=�$	��100��$	��100��.���� 	 �� Orientation$	 
Horizontal=�.+��� $��help.jpg��$��0,0,0,0q��� $��Button��