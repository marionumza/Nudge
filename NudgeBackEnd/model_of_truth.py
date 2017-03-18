import tensorflow as tf
import numpy as np
import pandas as pd
import csv



class Train:
    def __init__(self,inputs):
       pass

    #****************Train for new model improved model*************************
    def train(self):
        print('Loading csv file....')
        # Load datasets.
        training_set = tf.contrib.learn.datasets.base.load_csv_without_header(filename='Book1.csv',
                                                                              target_dtype=np.int,
                                                                              features_dtype=np.float32)
        print('File found and loaded')
        # Nameless feature columns for 4 real valued features
        feature_columns = [tf.contrib.layers.real_valued_column("", dimension=4)]

        # Build 3 layer DNN with 10, 20, 10 units respectively.
        self.__classifier = tf.contrib.learn.DNNClassifier(hidden_units=[10, 20, 10],
                                                    n_classes=2,
                                                    model_dir="first_model", feature_columns=feature_columns)
        print('Training Neural Network....')
        # Fit model.
        self.__classifier.fit(x=training_set.data,
                       y=training_set.target,
                       steps=10000)
        print('Training completed!')


    #*************Get array of probabilities of productive***********
    def getProbailities(self, inputs=None):
        predict_prob_out = self.__classifier.predict_proba(inputs, as_iterable=False)

        #Print the inputs to console
        print('Inputs: {}'.format(inputs))

        #Print prediction array [P(False),P(True)]
        print('Predictions: {}'.format(predict_prob_out))

        #Return probability of being
        return predict_prob_out[1]


    #***************Add input to csv*******************************
    def save_csv(self, features_labels=None):
        #Open csv in append mode and add new features with label
        with open('Book1.csv','a',newline='') as csvfile:
            writer = csv.writer(csvfile,delimiter=',')
            writer.writernow(features_labels)
