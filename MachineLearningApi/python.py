import pandas as pd
from sklearn.model_selection import train_test_split
from sklearn.ensemble import RandomForestClassifier
from sklearn.preprocessing import MultiLabelBinarizer, LabelEncoder
from sklearn.metrics import accuracy_score, classification_report

url = "main-dataset.csv"
data = pd.read_csv(url)

data = data.dropna()

data['Симптоми'] = data['Симптоми'].str.split(', ')

mlb_symptoms = MultiLabelBinarizer()
symptoms_encoded = mlb_symptoms.fit_transform(data['Симптоми'])

symptoms_df = pd.DataFrame(symptoms_encoded, columns=mlb_symptoms.classes_)
data = pd.concat([data, symptoms_df], axis=1)

data = data.drop('Симптоми', axis=1)

data['Комбинирано'] = data['Диагноза'] + '|' + data['Предотвратяване']

le = LabelEncoder()
data['Комбинирано'] = le.fit_transform(data['Комбинирано'])

X = data.drop(['Диагноза', 'Предотвратяване', 'Комбинирано'], axis=1)
y = data['Комбинирано']

X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

model = RandomForestClassifier(n_estimators=100, random_state=42)
model.fit(X_train, y_train)

y_pred = model.predict(X_test)
print("Accuracy:", accuracy_score(y_test, y_pred))
print(classification_report(y_test, y_pred))

import joblib
joblib.dump(model, 'symptoms_diagnosis_prevention_model.pkl')
joblib.dump(mlb_symptoms, 'mlb_symptoms.pkl')
joblib.dump(le, 'label_encoder.pkl')