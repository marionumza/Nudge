import NudgeBackEnd.model_of_truth as truth
import sys
import numpy as np

#Recive passed arguments
update = sys.argv

#Take the arg from the passed argument
update = update[1]

#Convert string to array of strings
update = eval('['+update+']')

#Convert array to numpy array
update = np.array(update, dtype='|S4')
update = update.astype(np.float)

#Convert numpy array to list
update = update.tolist()

#Store new labeled data point and train model again
truth.Train().save_csv(features_labels=update)

