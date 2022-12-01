FROM pytorch/pytorch:1.13.0-cuda11.6-cudnn8-runtime
WORKDIR /app
RUN pip3 install pydantic
RUN pip3 install fastapi
RUN pip3 install transformers
RUN pip3 install uvicorn
COPY . .
RUN python3 -c 'import Cache; Cache.downloadModels()'
CMD uvicorn main:app