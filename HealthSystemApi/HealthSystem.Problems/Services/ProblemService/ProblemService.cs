using HealthSystem.Problems.Data;
using HealthSystem.Problems.Data.Models;
using HealthSystem.Problems.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Problems.Services.ProblemService
{
    /// <summary>
    /// Service class for managing problem-related operations.
    /// Implements the <see cref="IProblemService"/> interface.
    /// </summary>
    public class ProblemService : IProblemService
    {
        private ProblemsDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProblemService"/> class.
        /// </summary>
        /// <param name="context">The database context for accessing problems and symptoms.</param>
        public ProblemService(ProblemsDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Adds a new problem to the database along with associated symptoms.
        /// </summary>
        /// <param name="problemAddModel">Model containing the details of the problem to add.</param>
        /// <param name="symptoms">List of symptom IDs to associate with the problem.</param>
        /// <param name="userId">ID of the user to whom the problem is associated (optional).</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains a boolean value indicating whether the problem was successfully added.</returns>
        public async Task<bool> AddAsync(ProblemAddModel problemAddModel, List<int> symptoms, string? userId)
        {
            var problem = new Problem()
            {
                Notes = problemAddModel.Notes,
                Date = problemAddModel.Date,
                UserId = userId
            };

            if (problemAddModel.HealthIssueId != 0)
            {
                problem.HealthIssueId = problemAddModel.HealthIssueId;
            }

            foreach (var id in symptoms)
            {
                var symptom = await context.Symptoms.FindAsync(id);
                problem.Symptoms.Add(symptom);
            }

            await context.Problems.AddAsync(problem);
            await context.SaveChangesAsync();

            return await context.Problems.ContainsAsync(problem);
        }

        /// <summary>
        /// Seeds the symptoms to the database.
        /// </summary>
        public async Task AddSymptomsAsync()
        {
            if (await context.SymptomCategories.AnyAsync())
            {
                return;
            }

            await context.SymptomCategories.AddRangeAsync(new List<SymptomCategory>()
                {
                    new SymptomCategory() { Name = "Общи симптоми" },
                    new SymptomCategory() { Name = "Нервни симптоми" },
                    new SymptomCategory() { Name = "Кожа, нокти и коса" },
                    new SymptomCategory() { Name = "Сърдечно-съдова и лимфна система" },
                    new SymptomCategory() { Name = "Дихателна система" },
                    new SymptomCategory() { Name = "Мускулно-скелетна система" },
                    new SymptomCategory() { Name = "Храносмилателна система" },
                    new SymptomCategory() { Name = "Пикочна система" },
                    new SymptomCategory() { Name = "Мъжка репродуктивна система" },
                    new SymptomCategory() { Name = "Женска репродуктивна система" },
                    new SymptomCategory() { Name = "Очи и уши" },
                    new SymptomCategory() { Name = "Психично здраве" },
                });

            await context.SaveChangesAsync();

            await context.SymptomSubCategories.AddRangeAsync(new List<SymptomSubCategory>()
            {
                new SymptomSubCategory() { Name = "Треска", CategoryId = 1 }, // 1
                new SymptomSubCategory() { Name = "Умора", CategoryId = 1 }, // 2
                new SymptomSubCategory() { Name = "Общо неразположение", CategoryId = 1 }, // 3
                new SymptomSubCategory() { Name = "Нарушение на течностния баланс", CategoryId = 1 }, // 4
                new SymptomSubCategory() { Name = "Липса на физиологично развитие", CategoryId = 1 }, // 5
                new SymptomSubCategory() { Name = "Наддаване на тегло", CategoryId = 1 }, // 6
                new SymptomSubCategory() { Name = "Отслабване", CategoryId = 1 }, // 7
                new SymptomSubCategory() { Name = "Болка, генерализирана, без конкретно място", CategoryId = 1 }, // 8
                new SymptomSubCategory() { Name = "Подуване или маса, без конкретно място", CategoryId = 1 }, // 9
                new SymptomSubCategory() { Name = "Общи симптоми при бебета и деца", CategoryId = 1 }, // 10
                new SymptomSubCategory() { Name = "Други общи симптоми", CategoryId = 1 }, // 11

                //-----Category 2-----
                new SymptomSubCategory() { Name = "Абнормални неволеви движения", CategoryId = 2 }, // 12
                new SymptomSubCategory() { Name = "Конвулсии", CategoryId = 2 }, // 13
                new SymptomSubCategory() { Name = "Главоболие", CategoryId = 2 }, // 14
                new SymptomSubCategory() { Name = "Нарушения на паметта", CategoryId = 2 }, // 15
                new SymptomSubCategory() { Name = "Други нарушения на усещанията", CategoryId = 2 }, // 15
                new SymptomSubCategory() { Name = "Нарушения на съня", CategoryId = 2 }, // 16
                new SymptomSubCategory() { Name = "Вертиго/световъртеж", CategoryId = 2 }, // 17
                new SymptomSubCategory() { Name = "Други нервни симптоми", CategoryId = 2 }, // 18
                //-----Категория 3-----
                new SymptomSubCategory() { Name = "Акне или пъпки", CategoryId = 3 }, // 19
                new SymptomSubCategory() { Name = "Обезцветяване или пигментация", CategoryId = 3 }, // 20
                new SymptomSubCategory() { Name = "Инфекциозни заболявания", CategoryId = 3 }, // 21
                new SymptomSubCategory() { Name = "Алергични кожни реакции", CategoryId = 3 }, // 22
                new SymptomSubCategory() { Name = "Кожни раздразнения", CategoryId = 3 }, // 23
                new SymptomSubCategory() { Name = "Подуване или маса на кожата", CategoryId = 3 }, // 24
                new SymptomSubCategory() { Name = "Рани на кожата", CategoryId = 3 }, // 25
                new SymptomSubCategory() { Name = "Други симптоми, свързани с кожата", CategoryId = 3 }, // 26
                new SymptomSubCategory() { Name = "Симптоми, свързани с ноктите", CategoryId = 3 }, // 27
                new SymptomSubCategory() { Name = "Симптоми, свързани с косата", CategoryId = 3 }, // 28
                new SymptomSubCategory() { Name = "Симптоми на пъпа", CategoryId = 3 }, // 29
                new SymptomSubCategory() { Name = "Други симптоми на кожа, нокти и коса", CategoryId = 3 }, // 30

                //-----Category 4-----
                new SymptomSubCategory() { Name = "Неправилни пулсации и сърцебиене", CategoryId = 4 }, // 31
                new SymptomSubCategory() { Name = "Абнормално високо кръвно налягане", CategoryId = 4 }, // 32
                new SymptomSubCategory() { Name = "Абнормално ниско кръвно налягане", CategoryId = 4 }, // 33
                new SymptomSubCategory() { Name = "Симптоми, свързани с кръвта", CategoryId = 4 }, // 34
                new SymptomSubCategory() { Name = "Бледност и цианоза", CategoryId = 4 }, // 35
                new SymptomSubCategory() { Name = "Синкоп или колапс", CategoryId = 4 }, // 36
                new SymptomSubCategory() { Name = "Симптоми на сърцето", CategoryId = 4 }, // 37
                new SymptomSubCategory() { Name = "Лимфни жлези", CategoryId = 4 }, // 38
                new SymptomSubCategory() { Name = "Други симптоми на сърдечно-съдовата и лимфната система", CategoryId = 4 }, // 39
                //-----Категория 5------
                new SymptomSubCategory() { Name = "Кръвотечение от носа", CategoryId = 5 }, // 40
                new SymptomSubCategory() { Name = "Назална конгестия", CategoryId = 5 }, // 41
                new SymptomSubCategory() { Name = "Проблеми със синусите", CategoryId = 5 }, // 42
                new SymptomSubCategory() { Name = "Дишане", CategoryId = 5 }, // 43
                new SymptomSubCategory() { Name = "Други нарушения на дихателния ритъм и звук", CategoryId = 5 }, // 44
                new SymptomSubCategory() { Name = "Грип", CategoryId = 5 }, // 45
                new SymptomSubCategory() { Name = "Храчки или секрети", CategoryId = 5 }, // 46
                new SymptomSubCategory() { Name = "Конгестия в гърдите", CategoryId = 5 }, // 47
                new SymptomSubCategory() { Name = "Болка в гърдите", CategoryId = 5 }, // 48
                new SymptomSubCategory() { Name = "Нарушения на гласа", CategoryId = 5 }, // 49
                new SymptomSubCategory() { Name = "Други симптоми на дихателната система", CategoryId = 5 }, // 50
                //-----Категория 6------
                new SymptomSubCategory() { Name = "Болка, подуване, травма на долен крайник", CategoryId = 6 }, // 51
                new SymptomSubCategory() { Name = "Болка, подуване, травма на горен крайник", CategoryId = 6 }, // 52
                new SymptomSubCategory() { Name = "Болка, подуване, травма на лицето и шията", CategoryId = 6 }, // 53
                new SymptomSubCategory() { Name = "Болка, подуване, травма на гръбната област", CategoryId = 6 }, // 54
                new SymptomSubCategory() { Name = "Атрофия или загуба на маса на крайниците", CategoryId = 6 }, // 55
                new SymptomSubCategory() { Name = "Трудност при ходене, нарушение на походката", CategoryId = 6 }, // 56
                new SymptomSubCategory() { Name = "Други симптоми на крайници и стави", CategoryId = 6 }, // 57
                new SymptomSubCategory() { Name = "Други симптоми на мускулно-скелетната система", CategoryId = 6 }, // 58

                new SymptomSubCategory() { Name = "Халитоза", CategoryId = 7 }, // 59
                new SymptomSubCategory() { Name = "Симптоми, свързани с устните", CategoryId = 7 }, // 60
                new SymptomSubCategory() { Name = "Симптоми, свързани с устата", CategoryId = 7 }, // 61
                new SymptomSubCategory() { Name = "Слюнка", CategoryId = 7 }, // 62
                new SymptomSubCategory() { Name = "Болка в гърлото", CategoryId = 7 }, // 63
                new SymptomSubCategory() { Name = "Симптоми, свързани с езика", CategoryId = 7 }, // 64
                new SymptomSubCategory() { Name = "Симптоми, свързани с сливиците", CategoryId = 7 }, // 65
                new SymptomSubCategory() { Name = "Гълтане", CategoryId = 7 }, // 66
                new SymptomSubCategory() { Name = "Коремна болка", CategoryId = 7 }, // 67
                new SymptomSubCategory() { Name = "Коремно подуване или маса", CategoryId = 7 }, // 68
                new SymptomSubCategory() { Name = "Метеоризъм", CategoryId = 7 }, // 69
                new SymptomSubCategory() { Name = "Ненормален апетит", CategoryId = 7 }, // 70
                new SymptomSubCategory() { Name = "Кървене, гастроинтестинално", CategoryId = 7 }, // 71
                new SymptomSubCategory() { Name = "Диария", CategoryId = 7 }, // 72
                new SymptomSubCategory() { Name = "Други симптоми или промени в чревната функция", CategoryId = 7 }, // 73
                new SymptomSubCategory() { Name = "Симптоми, свързани с ануса-ректума", CategoryId = 7 }, // 74
                new SymptomSubCategory() { Name = "Киселини или стомашно разстройство", CategoryId = 7 }, // 75
                new SymptomSubCategory() { Name = "Гадене и повръщане", CategoryId = 7 }, // 76
                new SymptomSubCategory() { Name = "Чернодробни и жлъчни проблеми", CategoryId = 7 }, // 77
                new SymptomSubCategory() { Name = "Други симптоми на храносмилателната система", CategoryId = 7 }, // 78
                //-----Категория 8-----
                new SymptomSubCategory() { Name = "Аномалии в урината", CategoryId = 8 }, // 79
                new SymptomSubCategory() { Name = "Честота и ноктурия", CategoryId = 8 }, // 80
                new SymptomSubCategory() { Name = "Инконтиненция на урината", CategoryId = 8 }, // 81
                new SymptomSubCategory() { Name = "Задържане на урина", CategoryId = 8 }, // 82
                new SymptomSubCategory() { Name = "Болезнено уриниране", CategoryId = 8 }, // 83
                new SymptomSubCategory() { Name = "Други симптоми на пикочните пътища", CategoryId = 8 }, // 84
                //-----Категория 9-----
                new SymptomSubCategory() { Name = "Безплодие - Мъжко", CategoryId = 9 }, // 85
                new SymptomSubCategory() { Name = "Болка, подуване или маса на мъжката полова система", CategoryId = 9 }, // 86
                new SymptomSubCategory() { Name = "Други симптоми на мъжката репродуктивна система", CategoryId = 9 }, // 87

                //-----Category 10-----
                new SymptomSubCategory() { Name = "Симптоми на менопауза", CategoryId = 10 }, // 88
                new SymptomSubCategory() { Name = "Менструални разстройства", CategoryId = 10 }, // 89
                new SymptomSubCategory() { Name = "Симптоми на таза", CategoryId = 10 }, // 90
                new SymptomSubCategory() { Name = "Вагинални разстройства", CategoryId = 10 }, // 91
                new SymptomSubCategory() { Name = "Вагинално отделяне", CategoryId = 10 }, // 92
                new SymptomSubCategory() { Name = "Вулварни разстройства", CategoryId = 10 }, // 93
                new SymptomSubCategory() { Name = "Безплодие - Женско", CategoryId = 10 }, // 94
                new SymptomSubCategory() { Name = "Проблеми по време на бременността", CategoryId = 10 }, // 95
                new SymptomSubCategory() { Name = "Топка или маса на гърдата", CategoryId = 10 }, // 96
                new SymptomSubCategory() { Name = "Болка или чувствителност на гърдата", CategoryId = 10 }, // 97
                new SymptomSubCategory() { Name = "Симптоми на брадавицата", CategoryId = 10 }, // 98
                new SymptomSubCategory() { Name = "Проблеми след раждане на гърдата", CategoryId = 10 }, // 99
                new SymptomSubCategory() { Name = "Други симптоми на гърдата", CategoryId = 10 }, // 100
                new SymptomSubCategory() { Name = "Други симптоми на женската репродуктивна система", CategoryId = 10 }, // 101
                //-----Категория 11-----
                new SymptomSubCategory() { Name = "Други дисфункции на зрението", CategoryId = 11 }, // 102
                new SymptomSubCategory() { Name = "Отделяне от окото", CategoryId = 11 }, // 103
                new SymptomSubCategory() { Name = "Болка и раздразнение в окото", CategoryId = 11 }, // 104
                new SymptomSubCategory() { Name = "Абнормални движения на окото", CategoryId = 11 }, // 105
                new SymptomSubCategory() { Name = "Симптоми на клепачите", CategoryId = 11 }, // 106
                new SymptomSubCategory() { Name = "Розово-око", CategoryId = 11 }, // 107
                new SymptomSubCategory() { Name = "Травми на окото", CategoryId = 11 }, // 108
                new SymptomSubCategory() { Name = "Абнормален външен вид на очите", CategoryId = 11 }, // 109
                new SymptomSubCategory() { Name = "Други дисфункции на слуха", CategoryId = 11 }, // 110
                new SymptomSubCategory() { Name = "Отделяне от ухото", CategoryId = 11 }, // 111
                new SymptomSubCategory() { Name = "Болка в ухото", CategoryId = 11 }, // 112
                new SymptomSubCategory() { Name = "Усещане за запушване в ухото", CategoryId = 11 }, // 113
                new SymptomSubCategory() { Name = "Други симптоми, свързани с ушите", CategoryId = 11 }, // 114
                new SymptomSubCategory() { Name = "Други симптоми на очите и ушите", CategoryId = 11 }, // 115
                //-----Категория 12-----
                new SymptomSubCategory() { Name = "Тревожност", CategoryId = 12 }, // 116
                new SymptomSubCategory() { Name = "Страхове и фобии", CategoryId = 12 }, // 117
                new SymptomSubCategory() { Name = "Депресия", CategoryId = 12 }, // 118
                new SymptomSubCategory() { Name = "Нервозност", CategoryId = 12 }, // 119
                new SymptomSubCategory() { Name = "Поведенчески разстройства", CategoryId = 12 }, // 120
                new SymptomSubCategory() { Name = "Проблеми, свързани с алкохола", CategoryId = 12 }, // 121
                new SymptomSubCategory() { Name = "Абнормална употреба на наркотици", CategoryId = 12 }, // 122
                new SymptomSubCategory() { Name = "Лоши навици", CategoryId = 12 }, // 123
                new SymptomSubCategory() { Name = "Психосексуални разстройства", CategoryId = 12 }, // 124
                new SymptomSubCategory() { Name = "Други проблеми с психичното здраве", CategoryId = 12 } // 125

            });

            await context.SaveChangesAsync();

            await context.Symptoms.AddRangeAsync(new List<Symptom>()
            {
                new Symptom() { Name = "Треска", CategoryId = 1 },
                new Symptom() { Name = "Висока температура", CategoryId = 1 },

                new Symptom() { Name = "Умора", CategoryId = 2 },
                new Symptom() { Name = "Изтощение", CategoryId = 2 },
                new Symptom() { Name = "Обща слабост", CategoryId = 2 },
                new Symptom() { Name = "Изключително уморен", CategoryId = 2 },
                new Symptom() { Name = "Изтощен", CategoryId = 2 },
                new Symptom() { Name = "Прегорял", CategoryId = 2 },
                new Symptom() { Name = "Износен", CategoryId = 2 },

                new Symptom() { Name = "Не се чувствам добре", CategoryId = 3 },

                new Symptom() { Name = "Обезводняване", CategoryId = 4 },
                new Symptom() { Name = "Прекомерно изпотяване", CategoryId = 4 },
                new Symptom() { Name = "Прекомерна жажда", CategoryId = 4 },
                new Symptom() { Name = "Задържане на течности", CategoryId = 4 },
                new Symptom() { Name = "Студена пот", CategoryId = 4 },

                new Symptom() { Name = "Липса на растеж", CategoryId = 5 },

                new Symptom() { Name = "Наднормено тегло", CategoryId = 6 },
                new Symptom() { Name = "Затлъстяване", CategoryId = 6 },

                new Symptom() { Name = "Наскоро загубено тегло", CategoryId = 7 },
                new Symptom() { Name = "Поднормено тегло", CategoryId = 7 },

                new Symptom() { Name = "Болка", CategoryId = 8 },
                new Symptom() { Name = "Болки навсякъде", CategoryId = 8 },
                new Symptom() { Name = "Спазъм", CategoryId = 8 },
                new Symptom() { Name = "Болка", CategoryId = 8 },
                new Symptom() { Name = "Стягане", CategoryId = 8 },

                new Symptom() { Name = "Изпъкналост", CategoryId = 9 },
                new Symptom() { Name = "Изпъкналиние", CategoryId = 9 },
                new Symptom() { Name = "Възел", CategoryId = 9 },
                new Symptom() { Name = "Топка", CategoryId = 9 },

                new Symptom() { Name = "Прекомерно плачене", CategoryId = 10 },
                new Symptom() { Name = "Неволни движения", CategoryId = 10 },
                new Symptom() { Name = "Раздразнителност", CategoryId = 10 },
                new Symptom() { Name = "Хиперактивност", CategoryId = 10 },
                new Symptom() { Name = "Раздразнителен", CategoryId = 10 },
                new Symptom() { Name = "Липса на активност", CategoryId = 10 },

                new Symptom() { Name = "Студени тръпки", CategoryId = 11 },

                new Symptom() { Name = "Треперене", CategoryId = 12 },
                new Symptom() { Name = "Тик", CategoryId = 12 },
                new Symptom() { Name = "Тремор", CategoryId = 12 },
                new Symptom() { Name = "Дърпане", CategoryId = 12 },

                new Symptom() { Name = "Припадъци", CategoryId = 13 },
                new Symptom() { Name = "Сепарации", CategoryId = 13 },
                new Symptom() { Name = "Спазми", CategoryId = 13 },

                new Symptom() { Name = "Главоболие", CategoryId = 14 },
                new Symptom() { Name = "Мигрена", CategoryId = 14 },
                new Symptom() { Name = "Нервозен", CategoryId = 14 },
                new Symptom() { Name = "Напрежение", CategoryId = 14 },

                new Symptom() { Name = "Амнезия", CategoryId = 15 },
                new Symptom() { Name = "Липса или загуба на паметта", CategoryId = 15 },
                new Symptom() { Name = "Временна загуба на паметта", CategoryId = 15 },

                new Symptom() { Name = "Анестезия", CategoryId = 16 },
                new Symptom() { Name = "Горене", CategoryId = 16 },
                new Symptom() { Name = "Хиперестезия", CategoryId = 16 },
                new Symptom() { Name = "Загуба на обоняние", CategoryId = 16 },
                new Symptom() { Name = "Загуба на вкус", CategoryId = 16 },
                new Symptom() { Name = "Загуба на допир", CategoryId = 16 },
                new Symptom() { Name = "Щипкане", CategoryId = 16 },
                new Symptom() { Name = "Покълване", CategoryId = 16 },

                new Symptom() { Name = "Сънливост", CategoryId = 17 },
                new Symptom() { Name = "Хиперсомния", CategoryId = 17 },
                new Symptom() { Name = "Безсъние", CategoryId = 17 },
                new Symptom() { Name = "Проблеми със съня", CategoryId = 17 },
                new Symptom() { Name = "Не мога да спя", CategoryId = 17 },
                new Symptom() { Name = "Кошмари", CategoryId = 17 },
                new Symptom() { Name = "Сънливост", CategoryId = 17 },
                new Symptom() { Name = "Сънна ходилка", CategoryId = 17 },
                new Symptom() { Name = "Синдром на времевата зона", CategoryId = 17 },

                new Symptom() { Name = "Въртене на главата-главоболие", CategoryId = 18 },
                new Symptom() { Name = "Усещане за падане", CategoryId = 18 },
                new Symptom() { Name = "Головокружение", CategoryId = 18 },
                new Symptom() { Name = "Леко главоболие", CategoryId = 18 },
                new Symptom() { Name = "Загуба на равновесие или баланс", CategoryId = 18 },

                new Symptom() { Name = "Кома и ступор", CategoryId = 19 },
                new Symptom() { Name = "Заблуда", CategoryId = 19 },
                new Symptom() { Name = "Старостеност", CategoryId = 19 },
                new Symptom() { Name = "Други симптоми, свързани с нервната система", CategoryId = 19 },

                new Symptom() { Name = "Лоша тена", CategoryId = 20 },
                new Symptom() { Name = "Черни точки", CategoryId = 20 },
                new Symptom() { Name = "Петна", CategoryId = 20 },
                new Symptom() { Name = "Излизане на белези", CategoryId = 20 },
                new Symptom() { Name = "Бели точки", CategoryId = 20 },

                new Symptom() { Name = "Поруменяване", CategoryId = 21 },
                new Symptom() { Name = "Промяна в цвета", CategoryId = 21 },
                new Symptom() { Name = "Покръвяване", CategoryId = 21 },
                new Symptom() { Name = "Пектилни петна", CategoryId = 21 },
                new Symptom() { Name = "Червеникави", CategoryId = 21 },
                new Symptom() { Name = "Петна", CategoryId = 21 },

                new Symptom() { Name = "Гъбичка на спортсмена", CategoryId = 22 },
                new Symptom() { Name = "Фуници", CategoryId = 22 },
                new Symptom() { Name = "Петно на гърба", CategoryId = 22 },

                new Symptom() { Name = "Обрив", CategoryId = 23 },
                new Symptom() { Name = "Копривна треска", CategoryId = 23 },
                new Symptom() { Name = "Фоточувствителност", CategoryId = 23 },
                new Symptom() { Name = "Отравяне с плющ, отрава с дъб и други", CategoryId = 23 },
                new Symptom() { Name = "Обрив, пелени", CategoryId = 23 },

                new Symptom() { Name = "Възпаление на кожата", CategoryId = 24 },
                new Symptom() { Name = "Сърбеж на кожата", CategoryId = 24 },
                new Symptom() { Name = "Болезнена кожа", CategoryId = 24 },
                new Symptom() { Name = "Чир на кожата", CategoryId = 24 },
                new Symptom() { Name = "Рана кожа", CategoryId = 24 },

                new Symptom() { Name = "Издутия кожен оброк", CategoryId = 25 },
                new Symptom() { Name = "Кожна лезия", CategoryId = 25 },
                new Symptom() { Name = "Кожни възли", CategoryId = 25 },
                new Symptom() { Name = "Кожни булавки", CategoryId = 25 },

                new Symptom() { Name = "Ухапвания по кожата", CategoryId = 26 },
                new Symptom() { Name = "Мехурчета по кожата, неалергични", CategoryId = 26 },
                new Symptom() { Name = "Кожни синини", CategoryId = 26 },
                new Symptom() { Name = "Опарения на кожата", CategoryId = 26 },
                new Symptom() { Name = "Нарязани нокти", CategoryId = 26 },
                new Symptom() { Name = "Драскотини на кожата", CategoryId = 26 },

                new Symptom() { Name = "Изсушаване на кожата", CategoryId = 27 },
                new Symptom() { Name = "Мазна кожа", CategoryId = 27 },
                new Symptom() { Name = "Лющене на кожата", CategoryId = 27 },
                new Symptom() { Name = "Размазаност на кожата", CategoryId = 27 },
                new Symptom() { Name = "Промяна в текстурата на кожата", CategoryId = 27 },

                new Symptom() { Name = "Ноктите се развалят", CategoryId = 28 },
                new Symptom() { Name = "Крехки нокти", CategoryId = 28 },
                new Symptom() { Name = "Промяна в цвета на ноктите", CategoryId = 28 },
                new Symptom() { Name = "Напукани нокти", CategoryId = 28 },
                new Symptom() { Name = "Врастнали нокти", CategoryId = 28 },
                new Symptom() { Name = "Бории по ноктите", CategoryId = 28 },
                new Symptom() { Name = "Ноктите се разцепват", CategoryId = 28 },

                new Symptom() { Name = "Оплешивяване", CategoryId = 29 },
                new Symptom() { Name = "Чуплива коса", CategoryId = 29 },
                new Symptom() { Name = "Суха коса", CategoryId = 29 },
                new Symptom() { Name = "Падаща коса", CategoryId = 29 },
                new Symptom() { Name = "Мазна коса", CategoryId = 29 },
                new Symptom() { Name = "Отстъпваща линия на косата", CategoryId = 29 },

                new Symptom() { Name = "Изтичане от пъпа", CategoryId = 30 },
                new Symptom() { Name = "Дренаж от пъпа", CategoryId = 30 },
                new Symptom() { Name = "Не заздравяващ пъп", CategoryId = 30 },
                new Symptom() { Name = "Болка в пъпа", CategoryId = 30 },
                new Symptom() { Name = "Червен пъп", CategoryId = 30 },

                new Symptom() { Name = "Мазоли или кокоши трън", CategoryId = 31 },
                new Symptom() { Name = "Кожни бенки", CategoryId = 31 },
                new Symptom() { Name = "Бръчки", CategoryId = 31 },
                new Symptom() { Name = "Кондиломи", CategoryId = 31 },

                new Symptom() { Name = "Намалени сърдечни удари", CategoryId = 32 },
                new Symptom() { Name = "Трептящо сърце", CategoryId = 32 },
                new Symptom() { Name = "Увеличени сърдечни удари", CategoryId = 32 },
                new Symptom() { Name = "Твърде бърз пулс", CategoryId = 32 },
                new Symptom() { Name = "Твърде бавен пулс", CategoryId = 32 },
                new Symptom() { Name = "Нередовни сърдечни удари", CategoryId = 32 },
                new Symptom() { Name = "Чуване на бързи сърдечни удари", CategoryId = 32 },
                new Symptom() { Name = "Пропуснат удар", CategoryId = 32 },
                new Symptom() { Name = "Неравномерни сърдечни удари", CategoryId = 32 },

                new Symptom() { Name = "Повишено кръвно налягане", CategoryId = 33 },
                new Symptom() { Name = "Високо кръвно налягане", CategoryId = 33 },
                new Symptom() { Name = "Хипертония", CategoryId = 33 },

                new Symptom() { Name = "Намалено кръвно налягане", CategoryId = 34 },
                new Symptom() { Name = "Хипотония", CategoryId = 34 },
                new Symptom() { Name = "Ниско кръвно налягане", CategoryId = 34 },

                new Symptom() { Name = "Лоша кръв", CategoryId = 35 },
                new Symptom() { Name = "Тънка кръв", CategoryId = 35 },
                new Symptom() { Name = "Умора от кръв", CategoryId = 35 },
                new Symptom() { Name = "Слаба кръв", CategoryId = 35 },

                new Symptom() { Name = "Сивкав цвят", CategoryId = 36 },
                new Symptom() { Name = "Синкавост на пръстите и пръстите на краката", CategoryId = 36 },
                new Symptom() { Name = "Бледност", CategoryId = 36 },

                new Symptom() { Name = "Преминаване в черно", CategoryId = 37 },
                new Symptom() { Name = "Припадък", CategoryId = 37 },
                new Symptom() { Name = "Загуба на съзнание", CategoryId = 37 },
                new Symptom() { Name = "Припадъци", CategoryId = 37 },

                new Symptom() { Name = "Лошо сърце", CategoryId = 38 },
                new Symptom() { Name = "Слабо сърце", CategoryId = 38 },
                new Symptom() { Name = "Слабо сърце", CategoryId = 38 },

                new Symptom() { Name = "Увеличени лимфни възли", CategoryId = 39 },
                new Symptom() { Name = "Болезнени жлези", CategoryId = 39 },
                new Symptom() { Name = "Отекли жлези", CategoryId = 39 },

                new Symptom() { Name = "Сърдечен шум", CategoryId = 40 },
                new Symptom() { Name = "Оток и воднянка", CategoryId = 40 },

                new Symptom() { Name = "Кървене от носа", CategoryId = 41 },
                new Symptom() { Name = "Хеморагия от носа", CategoryId = 41 },

                new Symptom() { Name = "Сечен нос", CategoryId = 42 },
                new Symptom() { Name = "Постназален дренаж", CategoryId = 42 },
                new Symptom() { Name = "Червен нос", CategoryId = 42 },
                new Symptom() { Name = "Течащ нос", CategoryId = 42 },
                new Symptom() { Name = "Снопи", CategoryId = 42 },
                new Symptom() { Name = "Запушен нос", CategoryId = 42 },

                new Symptom() { Name = "Запушени синуси", CategoryId = 43 },
                new Symptom() { Name = "Затлачени синуси", CategoryId = 43 },
                new Symptom() { Name = "Инфектиране на синусите", CategoryId = 43 },
                new Symptom() { Name = "Лекота в синусите", CategoryId = 43 },
                new Symptom() { Name = "Болка в синусите", CategoryId = 43 },
                new Symptom() { Name = "Налягане в синусите", CategoryId = 43 },

                new Symptom() { Name = "Задух", CategoryId = 44 },
                new Symptom() { Name = "Недостиг на въздух", CategoryId = 44 },
                new Symptom() { Name = "Диспнея", CategoryId = 44 },
                new Symptom() { Name = "Сензация на задушаване", CategoryId = 44 },
                new Symptom() { Name = "Проблеми с дишането", CategoryId = 44 },

                new Symptom() { Name = "Аномални дихателни звуци", CategoryId = 45 },
                new Symptom() { Name = "Хипервентилация", CategoryId = 45 },
                new Symptom() { Name = "Ръле", CategoryId = 45 },
                new Symptom() { Name = "Бързо дишане", CategoryId = 45 },
                new Symptom() { Name = "Сигнално дишане", CategoryId = 45 },
                new Symptom() { Name = "Свистене", CategoryId = 45 },

                new Symptom() { Name = "Грип", CategoryId = 46 },
                new Symptom() { Name = "Инфлуенца", CategoryId = 46 },

                new Symptom() { Name = "Кръв в храчките", CategoryId = 47 },
                new Symptom() { Name = "Изобилие от храчки", CategoryId = 47 },
                new Symptom() { Name = "Гнойна храчка", CategoryId = 47 },

                new Symptom() { Name = "Запушване на белите дробове", CategoryId = 48 },

                new Symptom() { Name = "Болка в гърдите", CategoryId = 49 },
                new Symptom() { Name = "Парене в гърдите", CategoryId = 49 },
                new Symptom() { Name = "Стягане в гърдите", CategoryId = 49 },
                new Symptom() { Name = "Болка в белия дроб", CategoryId = 49 },
                new Symptom() { Name = "Болка над сърцето", CategoryId = 49 },
                new Symptom() { Name = "Дихателна болка (в ребрата, ретростернално, стернално)", CategoryId = 49 },
                new Symptom() { Name = "Налягане в/на гърдите", CategoryId = 49 },

                new Symptom() { Name = "Хрипливост", CategoryId = 50 },
                new Symptom() { Name = "Хиперназалност", CategoryId = 50 },

                new Symptom() { Name = "Кихане", CategoryId = 51 },
                new Symptom() { Name = "Кашлица", CategoryId = 51 },
                new Symptom() { Name = "Студ", CategoryId = 51 },
                new Symptom() { Name = "Круп", CategoryId = 51 },

                new Symptom() { Name = "Болка в крака", CategoryId = 52 },
                new Symptom() { Name = "Мускулен спазъм в крака", CategoryId = 52 },
                new Symptom() { Name = "Контракция на крака", CategoryId = 52 },
                new Symptom() { Name = "Крампи в крака", CategoryId = 52 },
                new Symptom() { Name = "Чувство на топло-студено в крака", CategoryId = 52 },
                new Symptom() { Name = "Болка в крака", CategoryId = 52 },
                new Symptom() { Name = "Опънат мускул в крака", CategoryId = 52 },
                new Symptom() { Name = "Чувствителност в крака", CategoryId = 52 },
                new Symptom() { Name = "Спазми в крака", CategoryId = 52 },
                new Symptom() { Name = "Скованост в крака", CategoryId = 52 },
                new Symptom() { Name = "Опън в глезена, стъпалото, бедрото, коляното", CategoryId = 52 },
                new Symptom() { Name = "Опън в стъпалото", CategoryId = 52 },
                new Symptom() { Name = "Опън в бедрото", CategoryId = 52 },
                new Symptom() { Name = "Опън в коляното", CategoryId = 52 },
                new Symptom() { Name = "Опън в крака или бедрото", CategoryId = 52 },
                new Symptom() { Name = "Опън в долния крайник", CategoryId = 52 },
                new Symptom() { Name = "Опън в пръста", CategoryId = 52 },

                new Symptom() { Name = "Болка в ръката", CategoryId = 53 },
                new Symptom() { Name = "Контракция на ръката", CategoryId = 53 },
                new Symptom() { Name = "Чувство на топло-студено в ръката", CategoryId = 53 },
                new Symptom() { Name = "Болка в ръката", CategoryId = 53 },
                new Symptom() { Name = "Опънат мускул в ръката", CategoryId = 53 },
                new Symptom() { Name = "Чувствителност в ръката", CategoryId = 53 },
                new Symptom() { Name = "Спазъм в ръката", CategoryId = 53 },
                new Symptom() { Name = "Скованост в ръката", CategoryId = 53 },
                new Symptom() { Name = "Опън в ръката", CategoryId = 53 },
                new Symptom() { Name = "Опън в лакътя", CategoryId = 53 },
                new Symptom() { Name = "Опън в пръстите", CategoryId = 53 },
                new Symptom() { Name = "Опън в предмишницата", CategoryId = 53 },
                new Symptom() { Name = "Опън в ръката", CategoryId = 53 },
                new Symptom() { Name = "Опън в рамото", CategoryId = 53 },
                new Symptom() { Name = "Опън в палеца", CategoryId = 53 },
                new Symptom() { Name = "Опън в горната ръка", CategoryId = 53 },
                new Symptom() { Name = "Опън в горния крайник", CategoryId = 53 },
                new Symptom() { Name = "Опън в китката", CategoryId = 53 },

                new Symptom() { Name = "Болка в шията", CategoryId = 54 },
                new Symptom() { Name = "Болка в лицето", CategoryId = 54 },
                new Symptom() { Name = "Контракция на лицето", CategoryId = 54 },
                new Symptom() { Name = "Спазъм на шията", CategoryId = 54 },
                new Symptom() { Name = "Спазъм на лицето", CategoryId = 54 },
                new Symptom() { Name = "Болка в шията", CategoryId = 54 },
                new Symptom() { Name = "Болка в лицето", CategoryId = 54 },
                new Symptom() { Name = "Опънат мускул на шията", CategoryId = 54 },
                new Symptom() { Name = "Чувствителност на лицето", CategoryId = 54 },
                new Symptom() { Name = "Спазъм на лицето", CategoryId = 54 },
                new Symptom() { Name = "Скованост на шията", CategoryId = 54 },
                new Symptom() { Name = "Опън в задната част на главата", CategoryId = 54 },
                new Symptom() { Name = "Опън на цервикалния гръбнак", CategoryId = 54 },
                new Symptom() { Name = "Опън на лицето", CategoryId = 54 },
                new Symptom() { Name = "Опън на челюстта", CategoryId = 54 },
                new Symptom() { Name = "Опън на шията", CategoryId = 54 },
                new Symptom() { Name = "Опън на горния гръбнак", CategoryId = 54 },

                new Symptom() { Name = "Болка в гърба", CategoryId = 55 },
                new Symptom() { Name = "Контракция", CategoryId = 55 },
                new Symptom() { Name = "Спазъм в гърба", CategoryId = 55 },
                new Symptom() { Name = "Болка в гърба", CategoryId = 55 },
                new Symptom() { Name = "Опънат мускул в гърба", CategoryId = 55 },
                new Symptom() { Name = "Чувствителност в гърба", CategoryId = 55 },
                new Symptom() { Name = "Спазъм в гърба", CategoryId = 55 },
                new Symptom() { Name = "Скованост в гърба", CategoryId = 55 },
                new Symptom() { Name = "Опън в гърба", CategoryId = 55 },
                new Symptom() { Name = "Опън в гърба, горен, долен", CategoryId = 55 },
                new Symptom() { Name = "Опън в лумбалния гръбнак", CategoryId = 55 },
                new Symptom() { Name = "Опън в лумбосакралния гръбнак", CategoryId = 55 },
                new Symptom() { Name = "Опън в сакроилиачната област", CategoryId = 55 },
                new Symptom() { Name = "Опън в гръбнака", CategoryId = 55 },
                new Symptom() { Name = "Опън в торакалния гръбнак", CategoryId = 55 },

                new Symptom() { Name = "Изтръпване на крайниците", CategoryId = 56 },
                new Symptom() { Name = "Парализа, частична или пълна", CategoryId = 56 },
                new Symptom() { Name = "Слабост в крайниците", CategoryId = 56 },

                new Symptom() { Name = "Невнимателност при ходене", CategoryId = 57 },
                new Symptom() { Name = "Падане при ходене", CategoryId = 57 },
                new Symptom() { Name = "Невъзможност да стоиш или ходиш", CategoryId = 57 },
                new Symptom() { Name = "Куцање", CategoryId = 57 },
                new Symptom() { Name = "Похапване", CategoryId = 57 },

                new Symptom() { Name = "Халукс", CategoryId = 58 },

                new Symptom() { Name = "Лош дъх", CategoryId = 59 },

                new Symptom() { Name = "Ненормален цвят на устните", CategoryId = 61 },
                new Symptom() { Name = "Кървене от устните", CategoryId = 61 },
                new Symptom() { Name = "Напукани устни", CategoryId = 61 },
                new Symptom() { Name = "Сухи устни", CategoryId = 61 },
                new Symptom() { Name = "Болка в устните", CategoryId = 61 },
                new Symptom() { Name = "Отоци на устните", CategoryId = 61 },

                new Symptom() { Name = "Лош вкус", CategoryId = 62 },
                new Symptom() { Name = "Изгаряне в устата", CategoryId = 62 },
                new Symptom() { Name = "Суха уста", CategoryId = 62 },
                new Symptom() { Name = "Възпаление в устата", CategoryId = 62 },
                new Symptom() { Name = "Болка в устата", CategoryId = 62 },
                new Symptom() { Name = "Отоци в устата", CategoryId = 62 },
                new Symptom() { Name = "Язва в устата", CategoryId = 62 },

                new Symptom() { Name = "Прекалено много слюнка", CategoryId = 63 },
                new Symptom() { Name = "Липса на слюнка", CategoryId = 63 },
                new Symptom() { Name = "Течаща слюнка", CategoryId = 63 },

                new Symptom() { Name = "Болезнено гърло", CategoryId = 64 },
                new Symptom() { Name = "Скърцащо гърло", CategoryId = 64 },
                new Symptom() { Name = "Гърло с болка", CategoryId = 64 },

                new Symptom() { Name = "Ненормален цвят на езика", CategoryId = 65 },
                new Symptom() { Name = "Кървене от езика", CategoryId = 65 },
                new Symptom() { Name = "Везикули на езика", CategoryId = 65 },
                new Symptom() { Name = "Изгорял език", CategoryId = 65 },
                new Symptom() { Name = "Болка в езика", CategoryId = 65 },
                new Symptom() { Name = "Ръбчета на езика", CategoryId = 65 },
                new Symptom() { Name = "Гладък език", CategoryId = 65 },
                new Symptom() { Name = "Отоци или образувания на езика", CategoryId = 65 },
                new Symptom() { Name = "Язва на езика", CategoryId = 65 },

                new Symptom() { Name = "Кървене от сливиците", CategoryId = 66 },
                new Symptom() { Name = "Изтичане от сливиците", CategoryId = 66 },
                new Symptom() { Name = "Възпаление на сливиците", CategoryId = 66 },
                new Symptom() { Name = "Отоци на сливиците", CategoryId = 66 },

                new Symptom() { Name = "Трудности при преглъщане", CategoryId = 67 },
                new Symptom() { Name = "Задушаване", CategoryId = 67 },

                new Symptom() { Name = "Болка в корема", CategoryId = 68 },
                new Symptom() { Name = "Колики, чревни", CategoryId = 68 },
                new Symptom() { Name = "Болка в епигастриума", CategoryId = 68 },
                new Symptom() { Name = "Болка в илеалната област", CategoryId = 68 },
                new Symptom() { Name = "Болка в ингвиналната област", CategoryId = 68 },
                new Symptom() { Name = "Болка в десния/левия, горен/долен квадрант", CategoryId = 68 },
                new Symptom() { Name = "Болка в стомаха", CategoryId = 68 },
                new Symptom() { Name = "Болка в пъпа", CategoryId = 68 },

                new Symptom() { Name = "Коремна подуване", CategoryId = 69 },
                new Symptom() { Name = "Коремна пълнота", CategoryId = 69 },

                new Symptom() { Name = "Подуване, газове", CategoryId = 70 },
                new Symptom() { Name = "Подуване поради газ", CategoryId = 70 },
                new Symptom() { Name = "Излишък от газове", CategoryId = 70 },

                new Symptom() { Name = "Намален апетит", CategoryId = 71 },
                new Symptom() { Name = "Прекален апетит", CategoryId = 71 },
                new Symptom() { Name = "Загуба на апетит", CategoryId = 71 },

                new Symptom() { Name = "Кръв в изпражненията", CategoryId = 72 },
                new Symptom() { Name = "Хематемеза", CategoryId = 72 },
                new Symptom() { Name = "Кървене с неизвестна причина", CategoryId = 72 },
                new Symptom() { Name = "Повръщане на кръв", CategoryId = 72 },

                new Symptom() { Name = "Диария", CategoryId = 73 },
                new Symptom() { Name = "Разхлабени изпражнения", CategoryId = 73 },

                new Symptom() { Name = "Обемни изпражнения", CategoryId = 74 },
                new Symptom() { Name = "Тъмни изпражнения", CategoryId = 74 },
                new Symptom() { Name = "Мазни изпражнения", CategoryId = 74 },
                new Symptom() { Name = "Слизести изпражнения", CategoryId = 74 },
                new Symptom() { Name = "Гнойни изпражнения", CategoryId = 74 },
                new Symptom() { Name = "Ненормален цвят", CategoryId = 74 },
                new Symptom() { Name = "Ненормален мирис", CategoryId = 74 },

                new Symptom() { Name = "Кървене от ректума", CategoryId = 75 },
                new Symptom() { Name = "Сърбеж около ануса", CategoryId = 75 },
                new Symptom() { Name = "Образувание около ануса", CategoryId = 75 },
                new Symptom() { Name = "Болка в ректума", CategoryId = 75 },
                new Symptom() { Name = "Отоци около ануса", CategoryId = 75 },

                new Symptom() { Name = "Киселини или дискомфорт в стомаха", CategoryId = 76 },
                new Symptom() { Name = "Изпускане на въздух", CategoryId = 76 },
                new Symptom() { Name = "Неприятности с храносмилането", CategoryId = 76 },

                new Symptom() { Name = "Гадене", CategoryId = 77 },
                new Symptom() { Name = "Възвращане", CategoryId = 77 },
                new Symptom() { Name = "Стомахът ме боли", CategoryId = 77 },
                new Symptom() { Name = "Повръщане", CategoryId = 77 },
                new Symptom() { Name = "Повръщане", CategoryId = 77 },

                new Symptom() { Name = "Жлъчна колика", CategoryId = 78 },
                new Symptom() { Name = "Жлъчни камъни", CategoryId = 78 },

                new Symptom() { Name = "Трудности при дъвчене", CategoryId = 79 },
                new Symptom() { Name = "Кървене от венците", CategoryId = 79 },
                new Symptom() { Name = "Зъбобол", CategoryId = 79 },
                new Symptom() { Name = "Колики, детски", CategoryId = 79 },
                new Symptom() { Name = "Проблеми с храненето", CategoryId = 79 },
                new Symptom() { Name = "Запек", CategoryId = 79 },
                new Symptom() { Name = "Ретургитация или повръщане", CategoryId = 79 },
                new Symptom() { Name = "Хълцане", CategoryId = 79 },
                new Symptom() { Name = "Жълтеница", CategoryId = 79 },

                new Symptom() { Name = "Кръв в урината", CategoryId = 80 },
                new Symptom() { Name = "Гной в урината", CategoryId = 80 },
                new Symptom() { Name = "Ненормален цвят на урината", CategoryId = 80 },
                new Symptom() { Name = "Ненормален мирис на урината", CategoryId = 80 },

                new Symptom() { Name = "Напикаване", CategoryId = 81 },
                new Symptom() { Name = "Нощно изтичане", CategoryId = 81 },

                new Symptom() { Name = "Капене на урина", CategoryId = 82 },
                new Symptom() { Name = "Необходимост от уриниране без контрол", CategoryId = 82 },

                new Symptom() { Name = "Не мога да изпразня пикочния мехур", CategoryId = 83 },
                new Symptom() { Name = "Невъзможност да уринирам", CategoryId = 83 },

                new Symptom() { Name = "Болезнено уриниране", CategoryId = 84 },
                new Symptom() { Name = "Парене", CategoryId = 84 },

                new Symptom() { Name = "Проблеми с пикочния мехур", CategoryId = 85 },
                new Symptom() { Name = "Изминали бъбречни камъни", CategoryId = 85 },

                new Symptom() { Name = "Нисък брой сперматозоиди", CategoryId = 86 },
                new Symptom() { Name = "Стерилитет", CategoryId = 86 },

                new Symptom() { Name = "Болка в пениса", CategoryId = 87 },
                new Symptom() { Name = "Болка в скротума", CategoryId = 87 },
                new Symptom() { Name = "Болка в тестисите", CategoryId = 87 },
                new Symptom() { Name = "Отоци или образувание в пениса", CategoryId = 87 },
                new Symptom() { Name = "Отоци или образувание в скротума", CategoryId = 87 },
                new Symptom() { Name = "Отоци или образувание в тестисите", CategoryId = 87 },

                new Symptom() { Name = "Психосексуални проблеми", CategoryId = 88 },

                new Symptom() { Name = "Прилив на топлина", CategoryId = 89 },

                new Symptom() { Name = "Отсъствие на менструация", CategoryId = 90 },
                new Symptom() { Name = "Атипично менструално течение", CategoryId = 90 },
                new Symptom() { Name = "Менструални кръвни съсиреци", CategoryId = 90 },
                new Symptom() { Name = "Прекомерно менструално течение", CategoryId = 90 },
                new Symptom() { Name = "Честа менструация", CategoryId = 90 },
                new Symptom() { Name = "Редки менструации", CategoryId = 90 },
                new Symptom() { Name = "Нередовна менструация", CategoryId = 90 },
                new Symptom() { Name = "Голямо менструално течение", CategoryId = 90 },
                new Symptom() { Name = "Забавяне на менструацията", CategoryId = 90 },
                new Symptom() { Name = "Оскъдно менструално течение", CategoryId = 90 },
                new Symptom() { Name = "Малко менструално течение", CategoryId = 90 },

                new Symptom() { Name = "Коремна болка", CategoryId = 91 },
                new Symptom() { Name = "Коремно налягане или чувство на падање", CategoryId = 91 },
                new Symptom() { Name = "Коремно подуване или образувание", CategoryId = 91 },

                new Symptom() { Name = "Болка във влагалището", CategoryId = 92 },
                new Symptom() { Name = "Отоци или образувание във влагалището", CategoryId = 92 },

                new Symptom() { Name = "Атипичен вагинален секрет", CategoryId = 93 },
                new Symptom() { Name = "Кървав вагинален секрет", CategoryId = 93 },
                new Symptom() { Name = "Кафяв вагинален секрет", CategoryId = 93 },

                new Symptom() { Name = "Сърбеж на вулвата", CategoryId = 94 },
                new Symptom() { Name = "Болка на вулвата", CategoryId = 94 },
                new Symptom() { Name = "Отоци или образувания в перинеума", CategoryId = 94 },
                new Symptom() { Name = "Язва на вулвата", CategoryId = 94 },

                new Symptom() { Name = "Стерилитет", CategoryId = 95 },

                new Symptom() { Name = "Изтичане на околоплодна течност", CategoryId = 96 },
                new Symptom() { Name = "Възможен раждане", CategoryId = 96 },
                new Symptom() { Name = "Изход на продукти на зачеването", CategoryId = 96 },
                new Symptom() { Name = "Кървене", CategoryId = 96 },

                new Symptom() { Name = "Удар в гърдата", CategoryId = 97 },
                new Symptom() { Name = "Твърдо място в гърдата", CategoryId = 97 },
                new Symptom() { Name = "Възел в гърдата", CategoryId = 97 },
                new Symptom() { Name = "Местен оток на гърдата", CategoryId = 97 },
                new Symptom() { Name = "Нодула в гърдата", CategoryId = 97 },

                new Symptom() { Name = "Червенина на гърдата", CategoryId = 98 },
                new Symptom() { Name = "Общ оток на гърдата", CategoryId = 98 },
                new Symptom() { Name = "Чувствителност на гърдата", CategoryId = 98 },

                new Symptom() { Name = "Кървене от зърното", CategoryId = 99 },
                new Symptom() { Name = "Промяна в цвета на зърното", CategoryId = 99 },
                new Symptom() { Name = "Изтичане от зърното", CategoryId = 99 },
                new Symptom() { Name = "Възпаление на зърното", CategoryId = 99 },
                new Symptom() { Name = "Инверсия на зърното", CategoryId = 99 },

                new Symptom() { Name = "Ненормално изтичане от гърдата", CategoryId = 100 },
                new Symptom() { Name = "Отсъствие на мляко", CategoryId = 100 },
                new Symptom() { Name = "Трудност или невъзможност за кърмене", CategoryId = 100 },
                new Symptom() { Name = "Препълване на гърдата", CategoryId = 100 },
                new Symptom() { Name = "Прекомерно мляко", CategoryId = 100 },
                new Symptom() { Name = "Неправилно позициониране", CategoryId = 100 },

                new Symptom() { Name = "Упадък на гърдите", CategoryId = 101 },
                new Symptom() { Name = "Твърде големи гърди", CategoryId = 101 },
                new Symptom() { Name = "Твърде малки гърди", CategoryId = 101 },

                new Symptom() { Name = "Предменструално напрежение", CategoryId = 102 },
                new Symptom() { Name = "Менструални болки", CategoryId = 102 },
                new Symptom() { Name = "Болка при овулация", CategoryId = 102 },
                new Symptom() { Name = "Други симптоми на женската репродуктивна система", CategoryId = 102 },

                new Symptom() { Name = "Замъглено зрение", CategoryId = 103 },
                new Symptom() { Name = "Мъгливо зрение", CategoryId = 103 },
                new Symptom() { Name = "Забележимо намаляване на зрението", CategoryId = 103 },
                new Symptom() { Name = "Затъпено зрение", CategoryId = 103 },
                new Symptom() { Name = "Плаващи обекти в окото", CategoryId = 103 },
                new Symptom() { Name = "Полу-зрение", CategoryId = 103 },
                new Symptom() { Name = "Замъглено зрение", CategoryId = 103 },
                new Symptom() { Name = "Светлобоязън", CategoryId = 103 },
                new Symptom() { Name = "Петна в очите", CategoryId = 103 },

                new Symptom() { Name = "Кървене от окото", CategoryId = 104 },
                new Symptom() { Name = "Прекомерно сълзене от окото", CategoryId = 104 },
                new Symptom() { Name = "Гной от окото", CategoryId = 104 },
                new Symptom() { Name = "Сълзене на окото", CategoryId = 104 },

                new Symptom() { Name = "Парене в окото", CategoryId = 105 },
                new Symptom() { Name = "Възпаление на окото", CategoryId = 105 },
                new Symptom() { Name = "Дразнене на окото", CategoryId = 105 },
                new Symptom() { Name = "Сърбеж в окото", CategoryId = 105 },
                new Symptom() { Name = "Отоци или образувания на окото", CategoryId = 105 },

                new Symptom() { Name = "Ненормална реакция на окото", CategoryId = 106 },
                new Symptom() { Name = "Косогледство", CategoryId = 106 },
                new Symptom() { Name = "Неравни зеници", CategoryId = 106 },
                new Symptom() { Name = "Спазми на окото", CategoryId = 106 },
                new Symptom() { Name = "Притискане на окото", CategoryId = 106 },
                new Symptom() { Name = "Трептене на окото", CategoryId = 106 },

                new Symptom() { Name = "Упадък на клепача", CategoryId = 107 },
                new Symptom() { Name = "Възпаление на клепача", CategoryId = 107 },
                new Symptom() { Name = "Сърбеж на клепача", CategoryId = 107 },
                new Symptom() { Name = "Отоци или образувания на клепача", CategoryId = 107 },

                new Symptom() { Name = "Конюнктивит", CategoryId = 108 },
                new Symptom() { Name = "Червено око", CategoryId = 108 },

                new Symptom() { Name = "Черен очен", CategoryId = 109 },
                new Symptom() { Name = "Изгаряния на окото", CategoryId = 109 },
                new Symptom() { Name = "Наранявания", CategoryId = 109 },

                new Symptom() { Name = "Ненормално изпъкнало око", CategoryId = 110 },
                new Symptom() { Name = "Червени очи", CategoryId = 110 },
                new Symptom() { Name = "Мъгливи очи", CategoryId = 110 },
                new Symptom() { Name = "Затъпени очи", CategoryId = 110 },
                new Symptom() { Name = "Замъглени очи", CategoryId = 110 },

                new Symptom() { Name = "Остра загуба на слуха", CategoryId = 111 },
                new Symptom() { Name = "Намален слух", CategoryId = 111 },
                new Symptom() { Name = "Излишни шумове в ушите", CategoryId = 111 },
                new Symptom() { Name = "Шум в ушите", CategoryId = 111 },
                new Symptom() { Name = "Проблеми със слуха", CategoryId = 111 },

                new Symptom() { Name = "Гной от ухото", CategoryId = 112 },
                new Symptom() { Name = "Кървене от ухото", CategoryId = 112 },

                new Symptom() { Name = "Болка в ухото", CategoryId = 113 },

                new Symptom() { Name = "Запушени уши", CategoryId = 114 },
                new Symptom() { Name = "Счупване на ушите", CategoryId = 114 },
                new Symptom() { Name = "Пукот на ушите", CategoryId = 114 },
                new Symptom() { Name = "Затворени уши", CategoryId = 114 },

                new Symptom() { Name = "Чуждо тяло в ухото", CategoryId = 115 },
                new Symptom() { Name = "Сърбеж в ухото", CategoryId = 115 },
                new Symptom() { Name = "Отоци или образувания на ухото", CategoryId = 115 },

                new Symptom() { Name = "Пълна слепота", CategoryId = 116 },
                new Symptom() { Name = "Ечемик", CategoryId = 116 },
                new Symptom() { Name = "Чуждо тяло в окото", CategoryId = 116 },
                new Symptom() { Name = "Глухота", CategoryId = 116 },
                new Symptom() { Name = "Прекомерно восък в ухото", CategoryId = 116 },
                new Symptom() { Name = "Ненормален размер на ухото", CategoryId = 116 },

                new Symptom() { Name = "Тревожност", CategoryId = 117 },

                new Symptom() { Name = "Неспокойствие", CategoryId = 118 },
                new Symptom() { Name = "Хиперактивност", CategoryId = 118 },
                new Symptom() { Name = "Прекомерна активност", CategoryId = 118 },

                new Symptom() { Name = "Депресия, горчивина", CategoryId = 119 },
                new Symptom() { Name = "Депресия, прекомерно плачене", CategoryId = 119 },
                new Symptom() { Name = "Депресия, униние", CategoryId = 119 },
                new Symptom() { Name = "Депресия, недоволство", CategoryId = 119 },
                new Symptom() { Name = "Депресия, чувство на загуба", CategoryId = 119 },
                new Symptom() { Name = "Депресия, безнадеждност", CategoryId = 119 },
                new Symptom() { Name = "Депресия, нещастие", CategoryId = 119 },
                new Symptom() { Name = "Депресия, тревожност", CategoryId = 119 },

                new Symptom() { Name = "Пеперуди в стомаха", CategoryId = 120 },
                new Symptom() { Name = "Нерви", CategoryId = 120 },
                new Symptom() { Name = "Нервност, напрежение", CategoryId = 120 },
                new Symptom() { Name = "Нервност, смущение", CategoryId = 120 },

                new Symptom() { Name = "Антисоциално поведение", CategoryId = 121 },
                new Symptom() { Name = "Проблеми с поведението", CategoryId = 121 },
                new Symptom() { Name = "Раздразнителност", CategoryId = 121 },
                new Symptom() { Name = "Спорен характер", CategoryId = 121 },
                new Symptom() { Name = "Гневни изблици", CategoryId = 121 },

                new Symptom() { Name = "Алкохолизъм", CategoryId = 122 },
                new Symptom() { Name = "Пие твърде много", CategoryId = 122 },

                new Symptom() { Name = "Прекомерна употреба на стимуланти или депресанти", CategoryId = 123 },
                new Symptom() { Name = "Злоупотреба с медикаменти или наркотици", CategoryId = 123 },

                new Symptom() { Name = "Дъвчене на коса", CategoryId = 124 },
                new Symptom() { Name = "Хапане на нокти", CategoryId = 124 },
                new Symptom() { Name = "Суканица на палеца", CategoryId = 124 },

                new Symptom() { Name = "Фригидност", CategoryId = 125 },
                new Symptom() { Name = "Импотентност", CategoryId = 125 },

                new Symptom() { Name = "Самота", CategoryId = 126 },
                new Symptom() { Name = "Прекомерно пушене", CategoryId = 126 },
                new Symptom() { Name = "Илюзии или халюцинации", CategoryId = 126 },
                new Symptom() { Name = "Обръщения или компулсии", CategoryId = 126 },
            });

            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves the details of a problem by its ID.
        /// </summary>
        /// <param name="id">The ID of the problem to retrieve details for.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a <see cref="ProblemDetailsModel"/> with the problem details.</returns>
        public async Task<ProblemDetailsModel> DetailsAsync(int id)
        {
            var problem = await context.Problems
                .Include(p => p.Symptoms)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (problem == null)
            {
                return new ProblemDetailsModel();
            }

            return new ProblemDetailsModel()
            {
                Date = problem.Date,
                Notes = problem.Notes,
                Symptoms = problem.Symptoms.Select(s => new SymptomModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    CategoryName = context.SymptomSubCategories.FindAsync(s.CategoryId).Result.Name
                }).ToList()
            };
        }

        /// <summary>
        /// Edits an existing problem and updates its associated symptoms.
        /// </summary>
        /// <param name="problemEditModel">The model containing the updated problem details.</param>
        /// <param name="symptoms">The list of symptom IDs to associate with the updated problem.</param>
        /// <returns>A task representing the asynchronous operation. The task result is a boolean indicating whether the edit was successful.</returns>
        public async Task<bool> EditAsync(ProblemEditModel problemEditModel, List<int> symptoms)
        {
            var problem = await context.Problems
                .Include(p => p.Symptoms)
                .FirstOrDefaultAsync(p => p.Id == problemEditModel.Id);

            if (problem == null) return false;

            problem.Notes = problemEditModel.Notes;
            problem.Symptoms.Clear();

            foreach (var id in symptoms)
            {
                var symptom = await context.Symptoms.FindAsync(id);
                problem.Symptoms.Add(symptom);
            }

            await context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Loads all symptom categories along with their subcategories and symptoms for MAUI.
        /// </summary>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of <see cref="SymptomCategoryDisplayModel"/> representing the categories and their associated subcategories and symptoms.</returns>
        public async Task<List<SymptomCategoryDisplayModel>> LoadCategoriesForMAUI()
        {
            return await context.SymptomCategories
                .Select(category => new SymptomCategoryDisplayModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    SubCategories = category.SubCategories.Select(subCategory => new SymptomSubCategoryDisplayModel
                    {
                        Id = subCategory.Id,
                        Name = subCategory.Name,
                        Symptoms = subCategory.Symptoms.Select(symptom => new SymptomDisplayModel
                        {
                            Id = symptom.Id,
                            Name = symptom.Name
                        }).ToList()
                    }).ToList()
                }).ToListAsync();
        }

        /// <summary>
        /// Loads all symptom subcategories along with their associated symptoms for MAUI.
        /// </summary>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of <see cref="SymptomSubCategoryDisplayModel"/> representing the subcategories and their associated symptoms.</returns>
        public async Task<List<SymptomSubCategoryDisplayModel>> LoadSubCategoriesForMAUI()
        {
            return await context.SymptomSubCategories
                .Select(subCategory => new SymptomSubCategoryDisplayModel
                {
                    Id = subCategory.Id,
                    Name = subCategory.Name,
                    Symptoms = subCategory.Symptoms.Select(symptom => new SymptomDisplayModel
                    {
                        Id = symptom.Id,
                        Name = symptom.Name
                    }).ToList()
                }).ToListAsync();
        }

        /// <summary>
        /// Removes a problem by its ID.
        /// </summary>
        /// <param name="id">The ID of the problem to remove.</param>
        /// <returns>A task representing the asynchronous operation. The task result is a boolean indicating whether the removal was successful.</returns>
        public async Task<bool> RemoveAsync(int id)
        {
            var problem = new Problem() { Id = id };

            context.Problems.Remove(problem);
            await context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Retrieves the list of problems associated with a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose problems are being retrieved (optional).</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of <see cref="ProblemDisplayModel"/> representing the user's problems.</returns>
        public async Task<List<ProblemDisplayModel>> UserProblemsAsync(string? userId)
        {
            var problems = await context.Problems.Where(x => x.UserId == userId).ToListAsync();

            return problems.Select(x => new ProblemDisplayModel
            {
                Id = x.Id,
                Notes = x.Notes,
                Date = x.Date
            }).ToList();
        }
    }
}
