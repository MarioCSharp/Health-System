import pandas as pd
from sklearn.model_selection import train_test_split
from sklearn.ensemble import RandomForestClassifier
from sklearn.preprocessing import MultiLabelBinarizer, LabelEncoder
from sklearn.metrics import accuracy_score, classification_report
import joblib

url = "main-dataset.csv"
data = pd.read_csv(url)

data = data.dropna()

data['Симптоми'] = data['Симптоми'].str.split(', ')

def subtract_symptoms(symptom_lists):
    first = symptom_lists.iloc[0]
    rest = set(sum(symptom_lists.iloc[1:], []))
    return [symptom for symptom in first if symptom not in rest]

data_grouped = data.groupby('Диагноза').agg({
    'Симптоми': subtract_symptoms,
    'Предотвратяване': 'first',
    'Лекар': 'first'
}).reset_index()

mlb_symptoms = MultiLabelBinarizer()
symptoms_encoded = mlb_symptoms.fit_transform(data_grouped['Симптоми'])

symptoms_df = pd.DataFrame(symptoms_encoded, columns=mlb_symptoms.classes_)
data_grouped = pd.concat([data_grouped, symptoms_df], axis=1)

data_grouped = data_grouped.drop('Симптоми', axis=1)

data_grouped['Комбинирано'] = data_grouped['Диагноза'] + '|' + data_grouped['Предотвратяване'] + '|' + data_grouped['Лекар']

le = LabelEncoder()
data_grouped['Комбинирано'] = le.fit_transform(data_grouped['Комбинирано'])

X = data_grouped.drop(['Диагноза', 'Предотвратяване', 'Лекар', 'Комбинирано'], axis=1)
y = data_grouped['Комбинирано']

X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

model = RandomForestClassifier(n_estimators=100, random_state=42)
model.fit(X_train, y_train)

y_pred = model.predict(X_test)

print("Accuracy:", accuracy_score(y_test, y_pred))
print(classification_report(y_test, y_pred))

joblib.dump(model, 'symptoms_diagnosis_prevention_model.pkl')
joblib.dump(mlb_symptoms, 'mlb_symptoms.pkl')
joblib.dump(le, 'label_encoder.pkl')
