from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
import joblib
import pandas as pd
import numpy as np

if __name__ == "__main__":
    import uvicorn
    uvicorn.run("main:app", host="0.0.0.0", port=8080, reload=True)

model = joblib.load('symptoms_diagnosis_prevention_model.pkl')
mlb_symptoms = joblib.load('mlb_symptoms.pkl')
le = joblib.load('label_encoder.pkl')

app = FastAPI()

class SymptomsRequest(BaseModel):
    symptoms: list[str]

class PredictionResponse(BaseModel):
    diagnosis: str
    prevention: str
    probability: float
    doctorSpecialization: str

@app.post("/predict", response_model=list[PredictionResponse])
async def predict_symptoms(request: SymptomsRequest):
    try:
        symptoms_input = mlb_symptoms.transform([request.symptoms])
    except Exception as e:
        raise HTTPException(status_code=400, detail="Invalid symptoms provided.")

    input_encoded = pd.DataFrame(symptoms_input, columns=mlb_symptoms.classes_)
    
    X_columns = mlb_symptoms.classes_
    for column in X_columns:
        if column not in input_encoded.columns:
            input_encoded[column] = 0
    input_encoded = input_encoded[X_columns]

    probabilities = model.predict_proba(input_encoded)[0]
    
    top_n = 3 
    top_n_indices = np.argsort(probabilities)[-top_n:][::-1]
    top_n_predictions = le.inverse_transform(top_n_indices)
    
    response = []
    for idx, prediction in zip(top_n_indices, top_n_predictions):
        diagnosis, prevention, doctorSpecialization = prediction.split('|')
        response.append(PredictionResponse(
            diagnosis=diagnosis,
            prevention=prevention,
            probability=probabilities[idx],
            doctorSpecialization=doctorSpecialization
        ))
    
    return response