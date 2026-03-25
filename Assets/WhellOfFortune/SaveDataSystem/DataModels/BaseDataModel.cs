using System;


    public class BaseDataModel<T>
    {
        public string id;
        
        public virtual T Clone()
        {
             string json = this.ToJsonString();
            T returnedData = json.ToJsonObject<T>();
            return (T)Convert.ChangeType(returnedData, typeof(T));
        }
        
    }
