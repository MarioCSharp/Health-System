using HealthSystemApi.Data;
using HealthSystemApi.Data.Models;
using HealthSystemApi.Models.Problem;
using HealthSystemApi.Models.Symptom;
using Microsoft.EntityFrameworkCore;

namespace HealthSystemApi.Services.ProblemService
{
    public class ProblemService : IProblemService
    {
        private ApplicationDbContext context;

        public ProblemService(ApplicationDbContext context)
        {
            this.context = context;
        }

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

        public async Task AddSymptomsAsync()
        {
            if (await context.SymptomCategories.AnyAsync())
            {
                return;
            }

            await context.SymptomCategories.AddRangeAsync(new List<SymptomCategory>()
                {
                    new SymptomCategory() { Name = "General Symptoms" },
                    new SymptomCategory() { Name = "Nervous Symptoms" },
                    new SymptomCategory() { Name = "Skin, Nails and Hair" },
                    new SymptomCategory() { Name = "Cardiovascular and Lymphatic systems" },
                    new SymptomCategory() { Name = "Respiratory System" },
                    new SymptomCategory() { Name = "Musculoskeletal System" },
                    new SymptomCategory() { Name = "Digestive System" },
                    new SymptomCategory() { Name = "Urinary Tract" },
                    new SymptomCategory() { Name = "Male Reproductive System" },
                    new SymptomCategory() { Name = "Female Reproductive System" },
                    new SymptomCategory() { Name = "Eyes and Ears" },
                    new SymptomCategory() { Name = "Mental Health" },
                });

            await context.SaveChangesAsync();

            await context.SymptomSubCategories.AddRangeAsync(new List<SymptomSubCategory>()
            {
                new SymptomSubCategory() { Name = "Fever", CategoryId = 1 }, // 1
                new SymptomSubCategory() { Name = "Fatigue", CategoryId = 1 }, // 2
                new SymptomSubCategory() { Name = "General ill-feeling", CategoryId = 1 }, // 3
                new SymptomSubCategory() { Name = "Fluid imbalance", CategoryId = 1 }, // 4
                new SymptomSubCategory() { Name = "Lack of physiological development", CategoryId = 1 }, // 5
                new SymptomSubCategory() { Name = "Weight gain", CategoryId = 1 }, // 6
                new SymptomSubCategory() { Name = "Weight loss", CategoryId = 1 }, // 7
                new SymptomSubCategory() { Name = "Pain, generalized, site unspecifed", CategoryId = 1 }, // 8
                new SymptomSubCategory() { Name = "Swelling or mass, site unspecifed", CategoryId = 1 }, // 9
                new SymptomSubCategory() { Name = "General symptoms of infants and children", CategoryId = 1 }, // 10
                new SymptomSubCategory() { Name = "Other General Symptoms", CategoryId = 1 }, // 11
                //-----Category 2-----
                new SymptomSubCategory() { Name = "Abnormal involuntary movements", CategoryId = 2 }, // 12
                new SymptomSubCategory() { Name = "Convulsions", CategoryId = 2 }, // 13
                new SymptomSubCategory() { Name = "Headache", CategoryId = 2 }, // 14
                new SymptomSubCategory() { Name = "Memory, disturbances of", CategoryId = 2 }, // 15
                new SymptomSubCategory() { Name = "Other disturbances of sensation", CategoryId = 2 }, // 15
                new SymptomSubCategory() { Name = "Disturbances of sleep", CategoryId = 2 }, // 16
                new SymptomSubCategory() { Name = "Vertigo-dizzness", CategoryId = 2 }, // 17
                new SymptomSubCategory() { Name = "Other Nervous Symptoms", CategoryId = 2 }, // 18
                //-----Category 3-----
                new SymptomSubCategory() { Name = "Acne or pimples", CategoryId = 3 }, // 19
                new SymptomSubCategory() { Name = "Discoloration or pigmentation", CategoryId = 3 }, // 20
                new SymptomSubCategory() { Name = "Infectious disorders", CategoryId = 3 }, // 21
                new SymptomSubCategory() { Name = "Alergic skin reactions", CategoryId = 3 }, // 22
                new SymptomSubCategory() { Name = "Skin irritations", CategoryId = 3 }, // 23
                new SymptomSubCategory() { Name = "Swelling or mass of skin", CategoryId = 3 }, // 24
                new SymptomSubCategory() { Name = "Wounds of skin", CategoryId = 3 }, // 25
                new SymptomSubCategory() { Name = "Other symptoms referable to skin", CategoryId = 3 }, // 26
                new SymptomSubCategory() { Name = "Symptoms referable to nails", CategoryId = 3 }, // 27
                new SymptomSubCategory() { Name = "Symptoms referable to hair", CategoryId = 3 }, // 28
                new SymptomSubCategory() { Name = "Symptoms of umbilicus", CategoryId = 3 }, // 29
                new SymptomSubCategory() { Name = "Other Skin, Nails and Hair", CategoryId = 3 }, // 30
                //-----Category 4-----
                new SymptomSubCategory() { Name = "Irregular pulsations and palpitations", CategoryId = 4 }, // 31
                new SymptomSubCategory() { Name = "Abnormally high blood pressure", CategoryId = 4 }, // 32
                new SymptomSubCategory() { Name = "Abnormally low blood pressure", CategoryId = 4 }, // 33
                new SymptomSubCategory() { Name = "Symptoms referable to blood", CategoryId = 4 }, // 34
                new SymptomSubCategory() { Name = "Pallor and cyanosis", CategoryId = 4 }, // 35
                new SymptomSubCategory() { Name = "Syncope or collapse", CategoryId = 4 }, // 36
                new SymptomSubCategory() { Name = "Symptoms of heard", CategoryId = 4 }, // 37
                new SymptomSubCategory() { Name = "Lymp glands", CategoryId = 4 }, // 38
                new SymptomSubCategory() { Name = "Other Cardiovascular and Lymphatic systems", CategoryId = 4 }, // 39
                //-----Category 5------
                new SymptomSubCategory() { Name = "Nose bleed", CategoryId = 5 }, // 40
                new SymptomSubCategory() { Name = "Nasal congestion", CategoryId = 5 }, // 41
                new SymptomSubCategory() { Name = "Sinus problems", CategoryId = 5 }, // 42
                new SymptomSubCategory() { Name = "Breathing", CategoryId = 5 }, // 43
                new SymptomSubCategory() { Name = "Other disorders of respiratory rhythm and sound", CategoryId = 5 }, // 44
                new SymptomSubCategory() { Name = "Flu", CategoryId = 5 }, // 45
                new SymptomSubCategory() { Name = "Sputum or phlegm", CategoryId = 5 }, // 46
                new SymptomSubCategory() { Name = "Congestion in chest", CategoryId = 5 }, // 47
                new SymptomSubCategory() { Name = "Pain in chest", CategoryId = 5 }, // 48
                new SymptomSubCategory() { Name = "Disorders of voice", CategoryId = 5 }, // 49
                new SymptomSubCategory() { Name = "Other Respiratory System", CategoryId = 5 }, // 50
                //-----Category 6------
                new SymptomSubCategory() { Name = "Pain, swelling, injury of lower extremity", CategoryId = 6 }, // 51
                new SymptomSubCategory() { Name = "Pain, swelling, injury of upper extremity", CategoryId = 6 }, // 52
                new SymptomSubCategory() { Name = "Pain, swelling, injury of face and neck", CategoryId = 6 }, // 53
                new SymptomSubCategory() { Name = "Pain, swelling, injury of back region", CategoryId = 6 }, // 54
                new SymptomSubCategory() { Name = "Atrophy or wasting of extremities", CategoryId = 6 }, // 55
                new SymptomSubCategory() { Name = "Difficulty in walking, abnormality of gait", CategoryId = 6 }, // 56
                new SymptomSubCategory() { Name = "Other limb and joint symptoms", CategoryId = 6 }, // 57
                new SymptomSubCategory() { Name = "Other Musculoskeletal System", CategoryId = 6 }, // 58
                //-----Category 7------
                new SymptomSubCategory() { Name = "Halitosis", CategoryId = 7 }, // 59
                new SymptomSubCategory() { Name = "Symptoms referable to lips", CategoryId = 7 }, // 60
                new SymptomSubCategory() { Name = "Symptoms referable to mouth", CategoryId = 7 }, // 61
                new SymptomSubCategory() { Name = "Saliva", CategoryId = 7 }, // 62
                new SymptomSubCategory() { Name = "Throat soreness", CategoryId = 7 }, // 63
                new SymptomSubCategory() { Name = "Symptoms referable to tounge", CategoryId = 7 }, // 64
                new SymptomSubCategory() { Name = "Symptoms referable to tonsils", CategoryId = 7 }, // 65
                new SymptomSubCategory() { Name = "Swallowing", CategoryId = 7 }, // 66
                new SymptomSubCategory() { Name = "Abdominal pain", CategoryId = 7 }, // 67
                new SymptomSubCategory() { Name = "Abdominal swelling or mass", CategoryId = 7 }, // 68
                new SymptomSubCategory() { Name = "Flatulence", CategoryId = 7 }, // 69
                new SymptomSubCategory() { Name = "Appetite, abnormal", CategoryId = 7 }, // 70
                new SymptomSubCategory() { Name = "Bleedingm gastrointestinal", CategoryId = 7 }, // 71
                new SymptomSubCategory() { Name = "Diarrhea", CategoryId = 7 }, // 72
                new SymptomSubCategory() { Name = "Other symptoms or changes in bowel functions", CategoryId = 7 }, // 73
                new SymptomSubCategory() { Name = "Symptoms referable to anus-rectum", CategoryId = 7 }, // 74
                new SymptomSubCategory() { Name = "Heartburn or upset stomach", CategoryId = 7 }, // 75
                new SymptomSubCategory() { Name = "Nausea and vomiting", CategoryId = 7 }, // 76
                new SymptomSubCategory() { Name = "Liver and gallbladder", CategoryId = 7 }, // 77
                new SymptomSubCategory() { Name = "Other Digestive System", CategoryId = 7 }, // 78
                //-----Category 8-----
                new SymptomSubCategory() { Name = "Urine abnormalities", CategoryId = 8 }, // 79
                new SymptomSubCategory() { Name = "Frequency and nocturia", CategoryId = 8 }, // 80
                new SymptomSubCategory() { Name = "Incontinence of urine", CategoryId = 8 }, // 81
                new SymptomSubCategory() { Name = "Retention of urice", CategoryId = 8 }, // 82
                new SymptomSubCategory() { Name = "Painful urination", CategoryId = 8 }, // 83
                new SymptomSubCategory() { Name = "Other urinary tract symptoms", CategoryId = 8 }, // 84
                //-----Category 9-----
                new SymptomSubCategory() { Name = "Infertility - Male", CategoryId = 9 }, // 85
                new SymptomSubCategory() { Name = "Pain, swelling or mass male genital system", CategoryId = 9 }, // 86
                new SymptomSubCategory() { Name = "Other male reproductice system symptoms", CategoryId = 9 }, // 87
                //-----Category 10-----
                new SymptomSubCategory() { Name = "Menopause symptoms", CategoryId = 10 }, // 88
                new SymptomSubCategory() { Name = "Menstrual disorders", CategoryId = 10 }, // 89
                new SymptomSubCategory() { Name = "Pelvis symptoms", CategoryId = 10 }, // 90
                new SymptomSubCategory() { Name = "Vaginal disorders", CategoryId = 10 }, // 91
                new SymptomSubCategory() { Name = "Vaginal discharge", CategoryId = 10 }, // 92
                new SymptomSubCategory() { Name = "Vulvar disorders", CategoryId = 10 }, // 93
                new SymptomSubCategory() { Name = "Infertility - Female", CategoryId = 10 }, // 94
                new SymptomSubCategory() { Name = "Problems of pregrancy", CategoryId = 10 }, // 95
                new SymptomSubCategory() { Name = "Lump or mass of breast", CategoryId = 10 }, // 96
                new SymptomSubCategory() { Name = "Pain or soreness of breast", CategoryId = 10 }, // 97
                new SymptomSubCategory() { Name = "Symptoms of nipple", CategoryId = 10 }, // 98
                new SymptomSubCategory() { Name = "Postpartum problems of breast", CategoryId = 10 }, // 99
                new SymptomSubCategory() { Name = "Other breast symptoms", CategoryId = 10 }, // 100
                new SymptomSubCategory() { Name = "Other Female Reproductice System", CategoryId = 10 }, // 101
                //-----Category 11-----
                new SymptomSubCategory() { Name = "Other vision disfunction", CategoryId = 11 }, // 102
                new SymptomSubCategory() { Name = "Discharge from eye", CategoryId = 11 }, // 103
                new SymptomSubCategory() { Name = "Eye pain and irritation", CategoryId = 11 }, // 104
                new SymptomSubCategory() { Name = "Abnormal eye movements", CategoryId = 11 }, // 105
                new SymptomSubCategory() { Name = "Symptoms of eyelids", CategoryId = 11 }, // 106
                new SymptomSubCategory() { Name = "Pink-eye", CategoryId = 11 }, // 107
                new SymptomSubCategory() { Name = "Eye injuries", CategoryId = 11 }, // 108
                new SymptomSubCategory() { Name = "Abnormal appearance of eyes", CategoryId = 11 }, // 109
                new SymptomSubCategory() { Name = "Other hearing dysfunctions", CategoryId = 11 }, // 110
                new SymptomSubCategory() { Name = "Discharge from ear", CategoryId = 11 }, // 111
                new SymptomSubCategory() { Name = "Earache", CategoryId = 11 }, // 112
                new SymptomSubCategory() { Name = "Plugged feeling in ear", CategoryId = 11 }, // 113
                new SymptomSubCategory() { Name = "Other symptoms referable to ears", CategoryId = 11 }, // 114
                new SymptomSubCategory() { Name = "Other Eyes and Ears", CategoryId = 11 }, // 115
                //-----Category 12-----
                new SymptomSubCategory() { Name = "Anxiety", CategoryId = 12 }, // 116
                new SymptomSubCategory() { Name = "Fears and phobias", CategoryId = 12 }, // 117
                new SymptomSubCategory() { Name = "Depression", CategoryId = 12 }, // 118
                new SymptomSubCategory() { Name = "Nervousness", CategoryId = 12 }, // 119
                new SymptomSubCategory() { Name = "Behavorial distrurbance", CategoryId = 12 }, // 120
                new SymptomSubCategory() { Name = "Alchojol related problems", CategoryId = 12 }, // 121
                new SymptomSubCategory() { Name = "Abnormal drug usage", CategoryId = 12 }, // 122
                new SymptomSubCategory() { Name = "Bad habits", CategoryId = 12 }, // 123
                new SymptomSubCategory() { Name = "Psychosexual disorders", CategoryId = 12 }, // 124
                new SymptomSubCategory() { Name = "Other Mental Health", CategoryId = 12 } // 125
            });

            await context.SaveChangesAsync();

            await context.Symptoms.AddRangeAsync(new List<Symptom>()
            {
                new Symptom() { Name = "Fever", CategoryId = 1 },
                new Symptom() { Name = "High temperature", CategoryId = 1 },

                new Symptom() { Name = "Fatigue", CategoryId = 2 },
                new Symptom() { Name = "Exhausted", CategoryId = 2 },
                new Symptom() { Name = "General weakness", CategoryId = 2 },
                new Symptom() { Name = "Pooped", CategoryId = 2 },
                new Symptom() { Name = "Rundown", CategoryId = 2 },
                new Symptom() { Name = "Tired", CategoryId = 2 },
                new Symptom() { Name = "Worn out", CategoryId = 2 },

                new Symptom() { Name = "Not feeling well", CategoryId = 3 },

                new Symptom() { Name = "Dehydration", CategoryId = 4 },
                new Symptom() { Name = "Excessive sweating", CategoryId = 4 },
                new Symptom() { Name = "Excessive thirst", CategoryId = 4 },
                new Symptom() { Name = "Retention of fluid", CategoryId = 4 },
                new Symptom() { Name = "Cold sweat", CategoryId = 4 },

                new Symptom() { Name = "Lack of growth", CategoryId = 5 },

                new Symptom() { Name = "Overweight", CategoryId = 6 },
                new Symptom() { Name = "Obesity", CategoryId = 6 },

                new Symptom() { Name = "Recent weight loss", CategoryId = 7 },
                new Symptom() { Name = "Underweight", CategoryId = 7 },

                new Symptom() { Name = "Ache", CategoryId = 8 },
                new Symptom() { Name = "Aches all over", CategoryId = 8 },
                new Symptom() { Name = "Cramp", CategoryId = 8 },
                new Symptom() { Name = "Hurt", CategoryId = 8 },
                new Symptom() { Name = "Stiffness", CategoryId = 8 },

                new Symptom() { Name = "Bulge", CategoryId = 9 },
                new Symptom() { Name = "Bump", CategoryId = 9 },
                new Symptom() { Name = "Knot", CategoryId = 9 },
                new Symptom() { Name = "Lump", CategoryId = 9 },

                new Symptom() { Name = "Crying too much", CategoryId = 10 },
                new Symptom() { Name = "Fidgety", CategoryId = 10 },
                new Symptom() { Name = "Fussy", CategoryId = 10 },
                new Symptom() { Name = "Hyperactive", CategoryId = 10 },
                new Symptom() { Name = "Irritable", CategoryId = 10 },
                new Symptom() { Name = "Unceractive", CategoryId = 10 },

                new Symptom() { Name = "Chills", CategoryId = 11 },

                new Symptom() { Name = "Shaking", CategoryId = 12 },
                new Symptom() { Name = "Tic", CategoryId = 12 },
                new Symptom() { Name = "Tremor", CategoryId = 12 },
                new Symptom() { Name = "Twitch", CategoryId = 12 },

                new Symptom() { Name = "Fits", CategoryId = 13 },
                new Symptom() { Name = "Seizures", CategoryId = 13 },
                new Symptom() { Name = "Spells", CategoryId = 13 },

                new Symptom() { Name = "Headache", CategoryId = 14 },
                new Symptom() { Name = "Migraine", CategoryId = 14 },
                new Symptom() { Name = "Nervous", CategoryId = 14 },
                new Symptom() { Name = "Tension", CategoryId = 14 },

                new Symptom() { Name = "Amnesia", CategoryId = 15 },
                new Symptom() { Name = "Lack or loss memory", CategoryId = 15 },
                new Symptom() { Name = "Temporary loss of memory", CategoryId = 15 },

                new Symptom() { Name = "Anesthesia", CategoryId = 16 },
                new Symptom() { Name = "Burning", CategoryId = 16 },
                new Symptom() { Name = "Hyperesthesia", CategoryId = 16 },
                new Symptom() { Name = "Loss of smell", CategoryId = 16 },
                new Symptom() { Name = "Loss of taste", CategoryId = 16 },
                new Symptom() { Name = "Loss of touch", CategoryId = 16 },
                new Symptom() { Name = "Prickly feeling", CategoryId = 16 },
                new Symptom() { Name = "Tingling", CategoryId = 16 },

                new Symptom() { Name = "Drowsiness", CategoryId = 17 },
                new Symptom() { Name = "Hypersomnia", CategoryId = 17 },
                new Symptom() { Name = "Insomnia", CategoryId = 17 },
                new Symptom() { Name = "Trouble sleeping", CategoryId = 17 },
                new Symptom() { Name = "Can`t sleep", CategoryId = 17 },
                new Symptom() { Name = "Nightmares", CategoryId = 17 },
                new Symptom() { Name = "Sleepiness", CategoryId = 17 },
                new Symptom() { Name = "Sleep walking", CategoryId = 17 },
                new Symptom() { Name = "Time-zone syndrome", CategoryId = 17 },

                new Symptom() { Name = "Vertigo-dizziness", CategoryId = 18 },
                new Symptom() { Name = "Falling sensations", CategoryId = 18 },
                new Symptom() { Name = "Giddiness", CategoryId = 18 },
                new Symptom() { Name = "Lightheadedness", CategoryId = 18 },
                new Symptom() { Name = "Loss of sense of equilibrium or balance", CategoryId = 18 },

                new Symptom() { Name = "Coma and stupor", CategoryId = 19 },
                new Symptom() { Name = "Confusion", CategoryId = 19 },
                new Symptom() { Name = "Senility-Old Age", CategoryId = 19 },
                new Symptom() { Name = "Other symptoms referable to the nervous system", CategoryId = 19 },

                new Symptom() { Name = "Bad complexion", CategoryId = 20 },
                new Symptom() { Name = "Blackheads", CategoryId = 20 },
                new Symptom() { Name = "Blemishes", CategoryId = 20 },
                new Symptom() { Name = "Breaking out", CategoryId = 20 },
                new Symptom() { Name = "Whiteheads", CategoryId = 20 },

                new Symptom() { Name = "Blushing", CategoryId = 21 },
                new Symptom() { Name = "Change in color", CategoryId = 21 },
                new Symptom() { Name = "Flushing", CategoryId = 21 },
                new Symptom() { Name = "Freckles", CategoryId = 21 },
                new Symptom() { Name = "Red", CategoryId = 21 },
                new Symptom() { Name = "Spots", CategoryId = 21 },

                new Symptom() { Name = "Athlete`s foot", CategoryId = 22 },
                new Symptom() { Name = "Boils", CategoryId = 22 },
                new Symptom() { Name = "Ringworm", CategoryId = 22 },

                new Symptom() { Name = "Rash", CategoryId = 23 },
                new Symptom() { Name = "Hives", CategoryId = 23 },
                new Symptom() { Name = "Photosensitivity", CategoryId = 23 },
                new Symptom() { Name = "Posin ivy, poison oak, etc.", CategoryId = 23 },
                new Symptom() { Name = "Rash, diaper", CategoryId = 23 },

                new Symptom() { Name = "Skin inflammation", CategoryId = 24 },
                new Symptom() { Name = "Itching", CategoryId = 24 },
                new Symptom() { Name = "Painful skin", CategoryId = 24 },
                new Symptom() { Name = "Skin ulcer", CategoryId = 24 },
                new Symptom() { Name = "Sore skin", CategoryId = 24 },

                new Symptom() { Name = "Skin bumps", CategoryId = 25 },
                new Symptom() { Name = "Skin lesion", CategoryId = 25 },
                new Symptom() { Name = "Skin nodules", CategoryId = 25 },
                new Symptom() { Name = "Skin welts", CategoryId = 25 },

                new Symptom() { Name = "Bites on skin", CategoryId = 26 },
                new Symptom() { Name = "Blisters on skin, non-alergic", CategoryId = 26 },
                new Symptom() { Name = "Skin bruises", CategoryId = 26 },
                new Symptom() { Name = "Skin burns", CategoryId = 26 },
                new Symptom() { Name = "Skin cuts", CategoryId = 26 },
                new Symptom() { Name = "Skin scratches", CategoryId = 26 },

                new Symptom() { Name = "Skin dryness", CategoryId = 27 },
                new Symptom() { Name = "Skin oilness", CategoryId = 27 },
                new Symptom() { Name = "Skin peeling", CategoryId = 27 },
                new Symptom() { Name = "Skin scaliness", CategoryId = 27 },
                new Symptom() { Name = "Change in skin texture", CategoryId = 27 },

                new Symptom() { Name = "Breaking of nails", CategoryId = 28 },
                new Symptom() { Name = "Brittle nails", CategoryId = 28 },
                new Symptom() { Name = "Change in nails color", CategoryId = 28 },
                new Symptom() { Name = "Cracked nails", CategoryId = 28 },
                new Symptom() { Name = "Ingrown nails", CategoryId = 28 },
                new Symptom() { Name = "Ridges on nails", CategoryId = 28 },
                new Symptom() { Name = "Nails splitting", CategoryId = 28 },

                new Symptom() { Name = "Baldness", CategoryId = 29 },
                new Symptom() { Name = "Brittle hair", CategoryId = 29 },
                new Symptom() { Name = "Hair dryness", CategoryId = 29 },
                new Symptom() { Name = "Falling out hair", CategoryId = 29 },
                new Symptom() { Name = "Hair oilness", CategoryId = 29 },
                new Symptom() { Name = "Receding hair line", CategoryId = 29 },

                new Symptom() { Name = "Umblical discharge", CategoryId = 30 },
                new Symptom() { Name = "Umblical drainage", CategoryId = 30 },
                new Symptom() { Name = "Belly button not healing", CategoryId = 30 },
                new Symptom() { Name = "Belly button pain", CategoryId = 30 },
                new Symptom() { Name = "Red belly button", CategoryId = 30 },

                new Symptom() { Name = "Calluses or corns", CategoryId = 31 },
                new Symptom() { Name = "Skin moles", CategoryId = 31 },
                new Symptom() { Name = "Wrinkles", CategoryId = 31 },
                new Symptom() { Name = "Warts", CategoryId = 31 },

                new Symptom() { Name = "Decreased heart beats", CategoryId = 32 },
                new Symptom() { Name = "Fluttering heart", CategoryId = 32 },
                new Symptom() { Name = "Increased heart beats", CategoryId = 32 },
                new Symptom() { Name = "Pulse too fast", CategoryId = 32 },
                new Symptom() { Name = "Pulse too slow", CategoryId = 32 },
                new Symptom() { Name = "Iregullar hearh beats", CategoryId = 32 },
                new Symptom() { Name = "Rapid heart beats heard", CategoryId = 32 },
                new Symptom() { Name = "Skipped beat", CategoryId = 32 },
                new Symptom() { Name = "Unequal heart beats", CategoryId = 32 },

                new Symptom() { Name = "Elevated blood pressure", CategoryId = 33 },
                new Symptom() { Name = "High blood pressure", CategoryId = 33 },
                new Symptom() { Name = "Hypertension", CategoryId = 33 },

                new Symptom() { Name = "Decreased blood pressure", CategoryId = 34 },
                new Symptom() { Name = "Hypotension", CategoryId = 34 },
                new Symptom() { Name = "Low blood pressure", CategoryId = 34 },

                new Symptom() { Name = "Poor blood", CategoryId = 35 },
                new Symptom() { Name = "Thin blood", CategoryId = 35 },
                new Symptom() { Name = "Tired blood", CategoryId = 35 },
                new Symptom() { Name = "Weak blood", CategoryId = 35 },

                new Symptom() { Name = "Ashen color", CategoryId = 36 },
                new Symptom() { Name = "Blueness of figers-toes", CategoryId = 36 },
                new Symptom() { Name = "Paleness", CategoryId = 36 },

                new Symptom() { Name = "Blacked out", CategoryId = 37 },
                new Symptom() { Name = "Fainting", CategoryId = 37 },
                new Symptom() { Name = "Passed out", CategoryId = 37 },
                new Symptom() { Name = "Spells", CategoryId = 37 },

                new Symptom() { Name = "Bad heard", CategoryId = 38 },
                new Symptom() { Name = "Poor heard", CategoryId = 38 },
                new Symptom() { Name = "Weak heard", CategoryId = 38 },

                new Symptom() { Name = "Enlarged lymph nodes", CategoryId = 39 },
                new Symptom() { Name = "Sore glands", CategoryId = 39 },
                new Symptom() { Name = "Swollen glands", CategoryId = 39 },

                new Symptom() { Name = "Heart murmur", CategoryId = 40 },
                new Symptom() { Name = "Edema and dropsy", CategoryId = 40 },

                new Symptom() { Name = "Bleeding from nose", CategoryId = 41 },
                new Symptom() { Name = "Hemorrahage from nose", CategoryId = 41 },

                new Symptom() { Name = "Drippy nose", CategoryId = 42 },
                new Symptom() { Name = "Postnasal drip", CategoryId = 42 },
                new Symptom() { Name = "Red nose", CategoryId = 42 },
                new Symptom() { Name = "Runny nose", CategoryId = 42 },
                new Symptom() { Name = "Sniffles", CategoryId = 42 },
                new Symptom() { Name = "Stuffy nose", CategoryId = 42 },

                new Symptom() { Name = "Sinus cogestion", CategoryId = 43 },
                new Symptom() { Name = "Impacted sunises", CategoryId = 43 },
                new Symptom() { Name = "Infected sunises", CategoryId = 43 },
                new Symptom() { Name = "Lightness in sunises", CategoryId = 43 },
                new Symptom() { Name = "Pain in sunises", CategoryId = 43 },
                new Symptom() { Name = "Pressure in sunises", CategoryId = 43 },

                new Symptom() { Name = "Shortness of breath", CategoryId = 44 },
                new Symptom() { Name = "Breathlessness", CategoryId = 44 },
                new Symptom() { Name = "Dyspnes", CategoryId = 44 },
                new Symptom() { Name = "Sensation of suffocation", CategoryId = 44 },
                new Symptom() { Name = "Trouble breathing", CategoryId = 44 },

                new Symptom() { Name = "Abnormal breathing sounds", CategoryId = 45 },
                new Symptom() { Name = "Hyperventilation", CategoryId = 45 },
                new Symptom() { Name = "Rales", CategoryId = 45 },
                new Symptom() { Name = "Rapid breathing", CategoryId = 45 },
                new Symptom() { Name = "Sighing respiration", CategoryId = 45 },
                new Symptom() { Name = "Wheezing", CategoryId = 45 },

                new Symptom() { Name = "Grip", CategoryId = 46 },
                new Symptom() { Name = "Influenza", CategoryId = 46 },

                new Symptom() { Name = "Bloody sputum", CategoryId = 47 },
                new Symptom() { Name = "Excessive sputum", CategoryId = 47 },
                new Symptom() { Name = "Purulent sputum", CategoryId = 47 },

                new Symptom() { Name = "Lung cogestion", CategoryId = 48 },

                new Symptom() { Name = "Pain in chest", CategoryId = 49 },
                new Symptom() { Name = "Burning sensation in chest", CategoryId = 49 },
                new Symptom() { Name = "Chest tightness", CategoryId = 49 },
                new Symptom() { Name = "Pain in lung", CategoryId = 49 },
                new Symptom() { Name = "Pain over heart", CategoryId = 49 },
                new Symptom() { Name = "Respiratory pain(rib, retrosternal, sternal)", CategoryId = 49 },
                new Symptom() { Name = "Pressure in/on chest", CategoryId = 49 },

                new Symptom() { Name = "Voice hoarsenses", CategoryId = 50 },
                new Symptom() { Name = "Hypernasality", CategoryId = 50 },

                new Symptom() { Name = "Sneezing", CategoryId = 51 },
                new Symptom() { Name = "Cough", CategoryId = 51 },
                new Symptom() { Name = "Cold", CategoryId = 51 },
                new Symptom() { Name = "Croup", CategoryId = 51 },

                new Symptom() { Name = "Ache in leg", CategoryId = 52 },
                new Symptom() { Name = "Charley horse in leg", CategoryId = 52 },
                new Symptom() { Name = "Leg contracture", CategoryId = 52 },
                new Symptom() { Name = "Leg cramp", CategoryId = 52 },
                new Symptom() { Name = "Hot-cold leg feeling", CategoryId = 52 },
                new Symptom() { Name = "Leg hurt", CategoryId = 52 },
                new Symptom() { Name = "Pulled leg muscle", CategoryId = 52 },
                new Symptom() { Name = "Soreness in leg", CategoryId = 52 },
                new Symptom() { Name = "Spasm in leg", CategoryId = 52 },
                new Symptom() { Name = "Stiffness in leg", CategoryId = 52 },
                new Symptom() { Name = "Strain of ankle, foot, hip, knee", CategoryId = 52 },
                new Symptom() { Name = "Strain of foot", CategoryId = 52 },
                new Symptom() { Name = "Strain of hip", CategoryId = 52 },
                new Symptom() { Name = "Strain of knee", CategoryId = 52 },
                new Symptom() { Name = "Strain of leg or thigh", CategoryId = 52 },
                new Symptom() { Name = "Strain of lower extremity", CategoryId = 52 },
                new Symptom() { Name = "Strain of toe", CategoryId = 52 },

                new Symptom() { Name = "Ache in arm", CategoryId = 53 },
                new Symptom() { Name = "Arm contracture", CategoryId = 53 },
                new Symptom() { Name = "Hot-cold arm feeling", CategoryId = 53 },
                new Symptom() { Name = "Arm hurt", CategoryId = 53 },
                new Symptom() { Name = "Pulled arm muscle", CategoryId = 53 },
                new Symptom() { Name = "Soreness in arm", CategoryId = 53 },
                new Symptom() { Name = "Spasm in arm", CategoryId = 53 },
                new Symptom() { Name = "Stiffness in arm", CategoryId = 53 },
                new Symptom() { Name = "Strain of arm", CategoryId = 53 },
                new Symptom() { Name = "Strain of elbow", CategoryId = 53 },
                new Symptom() { Name = "Strain of fingers", CategoryId = 53 },
                new Symptom() { Name = "Strain of forearm", CategoryId = 53 },
                new Symptom() { Name = "Strain of hand", CategoryId = 53 },
                new Symptom() { Name = "Strain of shoulder", CategoryId = 53 },
                new Symptom() { Name = "Strain of thumb", CategoryId = 53 },
                new Symptom() { Name = "Strain of upper arm", CategoryId = 53 },
                new Symptom() { Name = "Strain of upper extremity", CategoryId = 53 },
                new Symptom() { Name = "Strain of wrist", CategoryId = 53 },

                new Symptom() { Name = "Ache in neck", CategoryId = 54 },
                new Symptom() { Name = "Ache in face", CategoryId = 54 },
                new Symptom() { Name = "Face contracture", CategoryId = 54 },
                new Symptom() { Name = "Neck cramp", CategoryId = 54 },
                new Symptom() { Name = "Face cramp", CategoryId = 54 },
                new Symptom() { Name = "Neck hurt", CategoryId = 54 },
                new Symptom() { Name = "Face hurt", CategoryId = 54 },
                new Symptom() { Name = "Pulled muscle of neck", CategoryId = 54 },
                new Symptom() { Name = "Soreness of face", CategoryId = 54 },
                new Symptom() { Name = "Face spasm", CategoryId = 54 },
                new Symptom() { Name = "Stiffness of neck", CategoryId = 54 },
                new Symptom() { Name = "Strain of back of head", CategoryId = 54 },
                new Symptom() { Name = "Strain of cervical spine", CategoryId = 54 },
                new Symptom() { Name = "Strain of face", CategoryId = 54 },
                new Symptom() { Name = "Strain of jaw", CategoryId = 54 },
                new Symptom() { Name = "Strain of neck", CategoryId = 54 },
                new Symptom() { Name = "Strain of upper spine", CategoryId = 54 },

                new Symptom() { Name = "Back ache", CategoryId = 55 },
                new Symptom() { Name = "Contracture", CategoryId = 55 },
                new Symptom() { Name = "Back cramp", CategoryId = 55 },
                new Symptom() { Name = "Back hurt", CategoryId = 55 },
                new Symptom() { Name = "Pulled back muscle", CategoryId = 55 },
                new Symptom() { Name = "Back soreness", CategoryId = 55 },
                new Symptom() { Name = "Back spasm", CategoryId = 55 },
                new Symptom() { Name = "Back stiffness", CategoryId = 55 },
                new Symptom() { Name = "Strain of back", CategoryId = 55 },
                new Symptom() { Name = "Strain of back, upper, lower", CategoryId = 55 },
                new Symptom() { Name = "Strain of lumbar", CategoryId = 55 },
                new Symptom() { Name = "Strain of lumbosacral", CategoryId = 55 },
                new Symptom() { Name = "Strain of sacroiliac", CategoryId = 55 },
                new Symptom() { Name = "Strain of spine", CategoryId = 55 },
                new Symptom() { Name = "Strain of thoracic spine", CategoryId = 55 },

                new Symptom() { Name = "Limb numbness", CategoryId = 56 },
                new Symptom() { Name = "Paralysis, partial or complete", CategoryId = 56 },
                new Symptom() { Name = "Limb weakness", CategoryId = 56 },

                new Symptom() { Name = "Clumsiness when walking", CategoryId = 57 },
                new Symptom() { Name = "Falling when walking", CategoryId = 57 },
                new Symptom() { Name = "Inability to stand or walk", CategoryId = 57 },
                new Symptom() { Name = "Limping", CategoryId = 57 },
                new Symptom() { Name = "Staggering", CategoryId = 57 },

                new Symptom() { Name = "Bunion", CategoryId = 58 },

                new Symptom() { Name = "Bad breath", CategoryId = 59 },

                new Symptom() { Name = "Abnormal lips color", CategoryId = 61 },
                new Symptom() { Name = "Bleeding lips", CategoryId = 61 },
                new Symptom() { Name = "Cracked lips", CategoryId = 61 },
                new Symptom() { Name = "Dry lips", CategoryId = 61 },
                new Symptom() { Name = "Lips pain", CategoryId = 61 },
                new Symptom() { Name = "Lips swelling", CategoryId = 61 },

                new Symptom() { Name = "Bad taste", CategoryId = 62 },
                new Symptom() { Name = "Mouth burn", CategoryId = 62 },
                new Symptom() { Name = "Mouth dryness", CategoryId = 62 },
                new Symptom() { Name = "Mouth inflammation", CategoryId = 62 },
                new Symptom() { Name = "Mouth pain", CategoryId = 62 },
                new Symptom() { Name = "Mouth swelling", CategoryId = 62 },
                new Symptom() { Name = "Mouth ucler", CategoryId = 62 },

                new Symptom() { Name = "Too much saliva", CategoryId = 63 },
                new Symptom() { Name = "Lack of saliva", CategoryId = 63 },
                new Symptom() { Name = "Drooling saliva", CategoryId = 63 },

                new Symptom() { Name = "Painful throat", CategoryId = 64 },
                new Symptom() { Name = "Scratchy throat", CategoryId = 64 },
                new Symptom() { Name = "Sore throat", CategoryId = 64 },

                new Symptom() { Name = "Abnormal tongue color", CategoryId = 65 },
                new Symptom() { Name = "Bleeding tongue", CategoryId = 65 },
                new Symptom() { Name = "Tongue blisters", CategoryId = 65 },
                new Symptom() { Name = "Burned tongue", CategoryId = 65 },
                new Symptom() { Name = "Tongue pain", CategoryId = 65 },
                new Symptom() { Name = "Tongue ridges", CategoryId = 65 },
                new Symptom() { Name = "Smooth tongue", CategoryId = 65 },
                new Symptom() { Name = "Swelling or mass in tongue", CategoryId = 65 },
                new Symptom() { Name = "Tongue ulcer", CategoryId = 65 },

                new Symptom() { Name = "Bleeding tonsils", CategoryId = 66 },
                new Symptom() { Name = "Tonsils discharge", CategoryId = 66 },
                new Symptom() { Name = "Tonsils inflammation", CategoryId = 66 },
                new Symptom() { Name = "Tonsils swelling", CategoryId = 66 },

                new Symptom() { Name = "Difficulty in swallowing", CategoryId = 67 },
                new Symptom() { Name = "Choking", CategoryId = 67 },

                new Symptom() { Name = "Abdominal pain", CategoryId = 68 },
                new Symptom() { Name = "Colic, intestinal", CategoryId = 68 },
                new Symptom() { Name = "Pain in Epigastrium", CategoryId = 68 },
                new Symptom() { Name = "Pain in Iliac", CategoryId = 68 },
                new Symptom() { Name = "Pain in Inguinal", CategoryId = 68 },
                new Symptom() { Name = "Pain in Right/Left, Upper/Lower quadrant", CategoryId = 68 },
                new Symptom() { Name = "Pain in Stomach", CategoryId = 68 },
                new Symptom() { Name = "Pain in Umbilical region", CategoryId = 68 },

                new Symptom() { Name = "Abdominal distension", CategoryId = 69 },
                new Symptom() { Name = "Abdominal fullness", CategoryId = 69 },

                new Symptom() { Name = "Bloating, gas", CategoryId = 70 },
                new Symptom() { Name = "Distension due to gas", CategoryId = 70 },
                new Symptom() { Name = "Gas, excessive", CategoryId = 70 },

                new Symptom() { Name = "Decreased appetite", CategoryId = 71 },
                new Symptom() { Name = "Excessive appetite", CategoryId = 71 },
                new Symptom() { Name = "Loss of appetite", CategoryId = 71 },

                new Symptom() { Name = "Blood in stools", CategoryId = 72 },
                new Symptom() { Name = "Hematemesis", CategoryId = 72 },
                new Symptom() { Name = "Hemorrhage, cause unknown", CategoryId = 72 },
                new Symptom() { Name = "Vomiting blood", CategoryId = 72 },

                new Symptom() { Name = "Diarrhea", CategoryId = 73 },
                new Symptom() { Name = "Loose stools", CategoryId = 73 },

                new Symptom() { Name = "Bulky stools", CategoryId = 74 },
                new Symptom() { Name = "Dark stools", CategoryId = 74 },
                new Symptom() { Name = "Fatty stools", CategoryId = 74 },
                new Symptom() { Name = "Mucous stools", CategoryId = 74 },
                new Symptom() { Name = "Pus stools", CategoryId = 74 },
                new Symptom() { Name = "Unusual color", CategoryId = 74 },
                new Symptom() { Name = "Unusual odor", CategoryId = 74 },

                new Symptom() { Name = "Rectal bleeding", CategoryId = 75 },
                new Symptom() { Name = "Anal itching", CategoryId = 75 },
                new Symptom() { Name = "Anal mass", CategoryId = 75 },
                new Symptom() { Name = "Rectal pain", CategoryId = 75 },
                new Symptom() { Name = "Anal swelling", CategoryId = 75 },

                new Symptom() { Name = "Heartburn or upset stomach", CategoryId = 76 },
                new Symptom() { Name = "Belching", CategoryId = 76 },
                new Symptom() { Name = "Indigestion", CategoryId = 76 },

                new Symptom() { Name = "Nausea", CategoryId = 77 },
                new Symptom() { Name = "Retching", CategoryId = 77 },
                new Symptom() { Name = "Sick to stomach", CategoryId = 77 },
                new Symptom() { Name = "Vomiting", CategoryId = 77 },
                new Symptom() { Name = "Throwing up", CategoryId = 77 },

                new Symptom() { Name = "Biliary colic", CategoryId = 78 },
                new Symptom() { Name = "Gallstones", CategoryId = 78 },

                new Symptom() { Name = "Chewing difficulty", CategoryId = 79 },
                new Symptom() { Name = "Bleeding, gums", CategoryId = 79 },
                new Symptom() { Name = "Toothache", CategoryId = 79 },
                new Symptom() { Name = "Colic, infantile", CategoryId = 79 },
                new Symptom() { Name = "Feeding problems", CategoryId = 79 },
                new Symptom() { Name = "Constipation", CategoryId = 79 },
                new Symptom() { Name = "Regurgitation or spitting-up", CategoryId = 79 },
                new Symptom() { Name = "Hiccough", CategoryId = 79 },
                new Symptom() { Name = "Jaundice", CategoryId = 79 },

                new Symptom() { Name = "Blood in urine", CategoryId = 80 },
                new Symptom() { Name = "Pus in urine", CategoryId = 80 },
                new Symptom() { Name = "Unusual urine color", CategoryId = 80 },
                new Symptom() { Name = "Unusual urine odor", CategoryId = 80 },

                new Symptom() { Name = "Bed wetting", CategoryId = 81 },
                new Symptom() { Name = "Night discharge", CategoryId = 81 },

                new Symptom() { Name = "Urinary dribbling", CategoryId = 82 },
                new Symptom() { Name = "Involuntary urination", CategoryId = 82 },

                new Symptom() { Name = "Cannot empty bladder", CategoryId = 83 },
                new Symptom() { Name = "Inability to urinate", CategoryId = 83 },

                new Symptom() { Name = "Painful urination", CategoryId = 84 },
                new Symptom() { Name = "Burning", CategoryId = 84 },

                new Symptom() { Name = "Bladder trouble", CategoryId = 85 },
                new Symptom() { Name = "Passed kidney stones", CategoryId = 85 },

                new Symptom() { Name = "Low sperm count", CategoryId = 86 },
                new Symptom() { Name = "Sterility", CategoryId = 86 },

                new Symptom() { Name = "Pain in penis", CategoryId = 87 },
                new Symptom() { Name = "Pain in scrotum", CategoryId = 87 },
                new Symptom() { Name = "Pain in testes", CategoryId = 87 },
                new Symptom() { Name = "Swelling, or mass in penis", CategoryId = 87 },
                new Symptom() { Name = "Swelling, or mass in scrotum", CategoryId = 87 },
                new Symptom() { Name = "Swelling, or mass in testes", CategoryId = 87 },

                new Symptom() { Name = "Psychosexual problems", CategoryId = 88 },

                new Symptom() { Name = "Hot flashes", CategoryId = 89 },

                new Symptom() { Name = "Menstruation absence", CategoryId = 90 },
                new Symptom() { Name = "Atypical menstrual flow", CategoryId = 90 },
                new Symptom() { Name = "Menstrual blood clots", CategoryId = 90 },
                new Symptom() { Name = "Excessive menstrual flow", CategoryId = 90 },
                new Symptom() { Name = "Frequant menstruation", CategoryId = 90 },
                new Symptom() { Name = "Infrequant menstruation", CategoryId = 90 },
                new Symptom() { Name = "Irregular menstruation", CategoryId = 90 },
                new Symptom() { Name = "Large menstrual flow", CategoryId = 90 },
                new Symptom() { Name = "Menstruation onset delay", CategoryId = 90 },
                new Symptom() { Name = "Scanty menstrual flow", CategoryId = 90 },
                new Symptom() { Name = "Small menstrual flow", CategoryId = 90 },

                new Symptom() { Name = "Pelvic pain", CategoryId = 91 },
                new Symptom() { Name = "Pelvic pressure or dropping sensation", CategoryId = 91 },
                new Symptom() { Name = "Pelvic swellin or mass", CategoryId = 91 },

                new Symptom() { Name = "Vaginal pain", CategoryId = 92 },
                new Symptom() { Name = "Vaginal swelling or mass", CategoryId = 92 },

                new Symptom() { Name = "Atypical vaginal discharge", CategoryId = 93 },
                new Symptom() { Name = "Bloody vaginal discharge", CategoryId = 93 },
                new Symptom() { Name = "Brown vaginal discharge", CategoryId = 93 },

                new Symptom() { Name = "Vulvar itching", CategoryId = 94 },
                new Symptom() { Name = "Vulvar pain", CategoryId = 94 },
                new Symptom() { Name = "Perineil swelling or mass", CategoryId = 94 },
                new Symptom() { Name = "Vulvar ulcer", CategoryId = 94 },

                new Symptom() { Name = "Sterility", CategoryId = 95 },

                new Symptom() { Name = "Leaking amniotic fluid", CategoryId = 96 },
                new Symptom() { Name = "Posible labor", CategoryId = 96 },
                new Symptom() { Name = "Products of conception passed", CategoryId = 96 },
                new Symptom() { Name = "Spotting", CategoryId = 96 },

                new Symptom() { Name = "Breast bump", CategoryId = 97 },
                new Symptom() { Name = "Breast hard spot", CategoryId = 97 },
                new Symptom() { Name = "Breast knot", CategoryId = 97 },
                new Symptom() { Name = "Local breast swelling", CategoryId = 97 },
                new Symptom() { Name = "Breast nodule", CategoryId = 97 },

                new Symptom() { Name = "Breast redness", CategoryId = 98 },
                new Symptom() { Name = "Breast swelling, generalized", CategoryId = 98 },
                new Symptom() { Name = "Breast tenderness", CategoryId = 98 },

                new Symptom() { Name = "Nipple bleeding", CategoryId = 99 },
                new Symptom() { Name = "Nipple change in color", CategoryId = 99 },
                new Symptom() { Name = "Nipple discharge", CategoryId = 99 },
                new Symptom() { Name = "Nipple inflammation", CategoryId = 99 },
                new Symptom() { Name = "Nipple inversion", CategoryId = 99 },

                new Symptom() { Name = "Abnormal breast secretion", CategoryId = 100 },
                new Symptom() { Name = "Absence of milk", CategoryId = 100 },
                new Symptom() { Name = "Difficulty or inability in nursing", CategoryId = 100 },
                new Symptom() { Name = "Breast engorgement", CategoryId = 100 },
                new Symptom() { Name = "Excessive mill", CategoryId = 100 },
                new Symptom() { Name = "Improper location", CategoryId = 100 },

                new Symptom() { Name = "Breast sagging", CategoryId = 101 },
                new Symptom() { Name = "Too large breasts", CategoryId = 101 },
                new Symptom() { Name = "Too small breasts", CategoryId = 101 },

                new Symptom() { Name = "Prenemenstrual tension", CategoryId = 102 },
                new Symptom() { Name = "Menstrual cramps", CategoryId = 102 },
                new Symptom() { Name = "Ovulation pain", CategoryId = 102 },
                new Symptom() { Name = "Other female reproductive system symptoms", CategoryId = 102 },

                new Symptom() { Name = "Blurred vision", CategoryId = 103 },
                new Symptom() { Name = "Clody vision", CategoryId = 103 },
                new Symptom() { Name = "Diminished vision", CategoryId = 103 },
                new Symptom() { Name = "Dull vision", CategoryId = 103 },
                new Symptom() { Name = "Eye floaters", CategoryId = 103 },
                new Symptom() { Name = "Half vision", CategoryId = 103 },
                new Symptom() { Name = "Hazy vision", CategoryId = 103 },
                new Symptom() { Name = "Photophobia", CategoryId = 103 },
                new Symptom() { Name = "Eye spots", CategoryId = 103 },

                new Symptom() { Name = "Blood eye discharge", CategoryId = 104 },
                new Symptom() { Name = "Excessive tearing from eye", CategoryId = 104 },
                new Symptom() { Name = "Pus from eye", CategoryId = 104 },
                new Symptom() { Name = "Watering eye", CategoryId = 104 },

                new Symptom() { Name = "Burning eye", CategoryId = 105 },
                new Symptom() { Name = "Eye inflamation", CategoryId = 105 },
                new Symptom() { Name = "Eye irritation", CategoryId = 105 },
                new Symptom() { Name = "Itchy eye", CategoryId = 105 },
                new Symptom() { Name = "Eye swelling or mass", CategoryId = 105 },

                new Symptom() { Name = "Abnormal eye reaction", CategoryId = 106 },
                new Symptom() { Name = "Cross-eyed", CategoryId = 106 },
                new Symptom() { Name = "Pupil unequal", CategoryId = 106 },
                new Symptom() { Name = "Eye spasms", CategoryId = 106 },
                new Symptom() { Name = "Eye squinting", CategoryId = 106 },
                new Symptom() { Name = "Eye twitching", CategoryId = 106 },

                new Symptom() { Name = "Drooping eyelid", CategoryId = 107 },
                new Symptom() { Name = "Eyelid inflammation", CategoryId = 107 },
                new Symptom() { Name = "Itchy eyelid", CategoryId = 107 },
                new Symptom() { Name = "Eyelid swelling or mass", CategoryId = 107 },

                new Symptom() { Name = "Pink-eye", CategoryId = 108 },
                new Symptom() { Name = "Conjunctivis", CategoryId = 108 },

                new Symptom() { Name = "Black eye", CategoryId = 109 },
                new Symptom() { Name = "Eye burns", CategoryId = 109 },
                new Symptom() { Name = "Scratches", CategoryId = 109 },

                new Symptom() { Name = "Abnormal eye protrusion", CategoryId = 110 },
                new Symptom() { Name = "Bloodshot eyes", CategoryId = 110 },
                new Symptom() { Name = "Cloudy eyes", CategoryId = 110 },
                new Symptom() { Name = "Dull eyes", CategoryId = 110 },
                new Symptom() { Name = "Hazy eyes", CategoryId = 110 },

                new Symptom() { Name = "Acute hearing", CategoryId = 111 },
                new Symptom() { Name = "Diminished hearing", CategoryId = 111 },
                new Symptom() { Name = "Extraneous noises in ears", CategoryId = 111 },
                new Symptom() { Name = "Ringing in ears", CategoryId = 111 },
                new Symptom() { Name = "Trouble hearing", CategoryId = 111 },

                new Symptom() { Name = "Pus from ear", CategoryId = 112 },
                new Symptom() { Name = "Ear bleeding", CategoryId = 112 },

                new Symptom() { Name = "Pain in ear", CategoryId = 113 },

                new Symptom() { Name = "Blocked ears", CategoryId = 114 },
                new Symptom() { Name = "Cracking ears", CategoryId = 114 },
                new Symptom() { Name = "Popping ears", CategoryId = 114 },
                new Symptom() { Name = "Stopped up ears", CategoryId = 114 },

                new Symptom() { Name = "Foreign body in ear", CategoryId = 115 },
                new Symptom() { Name = "Itchy ear", CategoryId = 115 },
                new Symptom() { Name = "Ear swelling or mass", CategoryId = 115 },

                new Symptom() { Name = "Complete blindness", CategoryId = 116 },
                new Symptom() { Name = "Sty", CategoryId = 116 },
                new Symptom() { Name = "Foreign body in eye", CategoryId = 116 },
                new Symptom() { Name = "Deafness", CategoryId = 116 },
                new Symptom() { Name = "Excess wax in ear", CategoryId = 116 },
                new Symptom() { Name = "Abnormal ear size", CategoryId = 116 },

                new Symptom() { Name = "Apprehension", CategoryId = 117 },

                new Symptom() { Name = "Restleness", CategoryId = 118 },
                new Symptom() { Name = "Hyperactivity", CategoryId = 118 },
                new Symptom() { Name = "Overactivity", CategoryId = 118 },

                new Symptom() { Name = "Depression, Bitterness", CategoryId = 119 },
                new Symptom() { Name = "Depression, Crying excessively", CategoryId = 119 },
                new Symptom() { Name = "Depression, Dejected", CategoryId = 119 },
                new Symptom() { Name = "Depression, Discontented", CategoryId = 119 },
                new Symptom() { Name = "Depression, Feeling lost", CategoryId = 119 },
                new Symptom() { Name = "Depression, Hopelessnes", CategoryId = 119 },
                new Symptom() { Name = "Depression, Unhappy", CategoryId = 119 },
                new Symptom() { Name = "Depression, Worrying", CategoryId = 119 },

                new Symptom() { Name = "Butterflies", CategoryId = 120 },
                new Symptom() { Name = "Nerves", CategoryId = 120 },
                new Symptom() { Name = "Nervousness, Tension", CategoryId = 120 },
                new Symptom() { Name = "Nervousness, Upset", CategoryId = 120 },

                new Symptom() { Name = "Antisocial behavior", CategoryId = 121 },
                new Symptom() { Name = "Behavorial problems", CategoryId = 121 },
                new Symptom() { Name = "Irritability", CategoryId = 121 },
                new Symptom() { Name = "Quarrelsome", CategoryId = 121 },
                new Symptom() { Name = "Temper tantrums", CategoryId = 121 },

                new Symptom() { Name = "Alchoholism", CategoryId = 122 },
                new Symptom() { Name = "Drinks too much", CategoryId = 122 },

                new Symptom() { Name = "Excessive use of stimulants or depressants", CategoryId = 123 },
                new Symptom() { Name = "Misuse of medications or drugs", CategoryId = 123 },

                new Symptom() { Name = "Chewing on hair", CategoryId = 124 },
                new Symptom() { Name = "Nail biting", CategoryId = 124 },
                new Symptom() { Name = "Thumb sucking", CategoryId = 124 },

                new Symptom() { Name = "Frigidity", CategoryId = 125 },
                new Symptom() { Name = "Impotence", CategoryId = 125 },

                new Symptom() { Name = "Loneliness", CategoryId = 126 },
                new Symptom() { Name = "Excessive smoking", CategoryId = 126 },
                new Symptom() { Name = "Delusions or hallucinations", CategoryId = 126 },
                new Symptom() { Name = "Obsessions or compulsions", CategoryId = 126 },
            });

            await context.SaveChangesAsync();
        }
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
                HealthIssueId = problem.HealthIssueId,
                Symptoms = problem.Symptoms.Select(s => new SymptomModel
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList()
            };
        }

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

        public async Task<bool> RemoveAsync(int id)
        {
            var problem = await context.Problems.FindAsync(id);

            if (problem is null) return false;

            context.Problems.Remove(problem);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<List<ProblemDisplayModel>> UserProblemsAsync(string? userId)
        {
            var problems = await context.Problems.Where(x => x.UserId == userId).Include(x => x.Symptoms).ToListAsync();

            return problems.Select(x => new ProblemDisplayModel
            {
                Notes = x.Notes,
                Date = x.Date,
                Symptoms = x.Symptoms.Select(y => new SymptomModel()
                {
                    Id = y.Id,
                    Name = y.Name
                }).ToList()
            }).ToList();
        }
    }
}