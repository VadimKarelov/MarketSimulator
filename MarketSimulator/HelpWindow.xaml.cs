using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MarketSimulator
{
    /// <summary>
    /// Логика взаимодействия для HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();
            SetText();
        }

        private void SetText()
        {
            label_Text.Content = @"Для запуска и остановки модели используется кнопка старт/пауза.

Поведение модели:
1.В начале дня у всех продавцов есть товар по определенной цене.
У покупателей товара нет.
2.Покупатель случайным образом выбирает продавца, с которым
будет проведена торговля.
3.Если покупателя устроила цена, то он забирает товар,
иначе идет к другому продавцу.
4.В конце дня подводятся итоги. Если продавец смог продать товар,
то он повышает цену на свой товар, иначе понижает.
                
Влияние на модель:
1.Можно менять количество покупателей и продавцов.
2.Также можно создавать внешние условия: снижать и повышать
цену на товары у продавцов и максимальную цену для покупателей.

Визуализация модели:
Зеленые квадраты - продавцы
Синие квадраты - покупатели
Красный круг - товар";
        }
    }
}
