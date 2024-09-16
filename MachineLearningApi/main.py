from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
import joblib
import pandas as pd

model = joblib.load('symptoms_diagnosis_prevention_model.pkl')
mlb_symptoms = joblib.load('mlb_symptoms.pkl')
le = joblib.load('label_encoder.pkl')

app = FastAPI()

class SymptomsRequest(BaseModel):
    symptoms: str 

class PredictionResponse(BaseModel):
    diagnosis: str
    prevention: str
    doctorSpecialization: str

@app.post("/predict", response_model=PredictionResponse)
async def predict_symptoms(request: SymptomsRequest):
    symptoms_list = [symptom.strip() for symptom in request.symptoms.split(', ')]

    valid_symptoms = [symptom for symptom in symptoms_list if symptom in mlb_symptoms.classes_]

    if not valid_symptoms:
        raise HTTPException(status_code=400, detail="No valid symptoms found in the request.")

    try:
        symptoms_encoded = mlb_symptoms.transform([valid_symptoms])
        symptoms_df = pd.DataFrame(symptoms_encoded, columns=mlb_symptoms.classes_)

        for col in model.feature_names_in_:
            if col not in symptoms_df.columns:
                symptoms_df[col] = 0

        symptoms_df = symptoms_df[model.feature_names_in_]

        prediction = model.predict(symptoms_df)[0]

        combined_label = le.inverse_transform([prediction])[0]

        diagnosis, prevention, doctor = combined_label.split('|')

        return PredictionResponse(
            diagnosis=diagnosis,
            prevention=prevention,
            doctorSpecialization=doctor
        )

    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Error in prediction: {e}")
    
@app.get("/columns")
async def get_columns():
    try:
        symptoms_encoded = mlb_symptoms.transform([[]]) 
        df = pd.DataFrame(symptoms_encoded, columns=mlb_symptoms.classes_)

        columns_to_return = df.columns[2:-2].tolist()

        return {"columns": columns_to_return}

    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Error retrieving columns: {e}")

if __name__ == "__main__":
    import uvicorn
    uvicorn.run("main:app", host="0.0.0.0", port=8080, reload=True)