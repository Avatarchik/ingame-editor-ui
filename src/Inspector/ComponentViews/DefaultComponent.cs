﻿/*==== DefaultComponent.cs ====================================================
 * DefaultComponent is a general Component representation
 * 
 * Author: Victor Le aka "Coac"
 * Repository : https://github.com/Coac/ingame-editor-ui.git
 * =========================================================================*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

public class DefaultComponent : AbstractComponent
{
    private string text;

    private bool displayField = false;
    private System.Reflection.FieldInfo[] fieldInfo;


    private bool displayProps = false;
    System.Reflection.PropertyInfo[] propertyInfo;

    private bool displayMethod = false;
    private string methods = "";

    public DefaultComponent(Component co)
    {
        this.co = co;

        Type t = co.GetType();
        this.text += "\nType " + t;
        this.text += "\nType information for:" + t.FullName;
        this.text += "\n\tBase class = " + t.BaseType.FullName;
        this.text += "\n\tIs Class = " + t.IsClass;
        this.text += "\n\tIs Enum = " + t.IsEnum;
        this.text += "\n\tAttributes = " + t.Attributes;
        this.fieldInfo = t.GetFields();
        this.propertyInfo = t.GetProperties();

        MethodInfo[] methodInfos = t.GetMethods(BindingFlags.Public | BindingFlags.Instance);
        foreach (MethodInfo methodInfo in methodInfos)
        {
            this.methods += "\n" + methodInfo.ReturnType.Name + " " + methodInfo.Name + "(";
            foreach (ParameterInfo p in methodInfo.GetParameters())
            {
                this.methods += p.ParameterType.Name + " " + p.Name + ", ";
            }
            this.methods += ")";
            
        }
         
    }


    public override void draw()
    {
        if(co == null) { return; }

        base.draw();

        this.drawDefaultComponent();
    }


    private void drawDefaultComponent()
    {
        GUILayout.TextArea(this.text);


        if (this.fieldInfo.Length > 0 && GUILayout.Button("Fields", GUILayout.Height(16)))
        {
            this.displayField = !this.displayField;
        }
        if (displayField)
        {
            foreach (System.Reflection.FieldInfo info in fieldInfo)
            {
                GUILayout.BeginHorizontal();

                GUILayout.Label(info.Name);

                object value = info.GetValue(this.co);
                switch (info.FieldType.ToString())
                {
                    case "System.Boolean":
                        info.SetValue(this.co, FieldView.displayBool((bool)value));
                        break;
                    case "UnityEngine.Vector3":
                        info.SetValue(this.co, FieldView.displayVector3((Vector3)value));
                        break;
                    case "System.Int32":
                        info.SetValue(this.co, FieldView.displayInt((int)value));
                        break;
                    case "System.Single":
                        info.SetValue(this.co, FieldView.displayFloat((float)value));
                        break;
                    case "System.Double":
                        info.SetValue(this.co, FieldView.displayDouble((double)value));
                        break;
                    case "System.String":
                        info.SetValue(this.co, FieldView.displayString((string)value));
                        break;
                    case "System.Collections.Generic.List`1[System.String]":
                        info.SetValue(this.co, FieldView.displayListString((List<string>)value));
                        break;
                    default:

                        if (value == null)
                        {
                            GUILayout.Label("null");
                        }
                        else
                        {
                            GUILayout.Label(value.ToString());
                        }

                        if (info.FieldType.ToString().Contains("System.Collections.Generic.List`1"))
                        {
                            try
                            {
                                IEnumerable list = (IEnumerable)value;
                                GUILayout.BeginVertical();
                                foreach (var str in list)
                                {
                                    if (str == null) continue;

                                    GUILayout.Label(str.ToString());
                                }
                                GUILayout.EndVertical();
                            }
                            catch { }
                        }

                        break;
                }

                GUILayout.EndHorizontal();
            }
        }

        if (this.propertyInfo.Length > 0 && GUILayout.Button("Props", GUILayout.Height(16)))
        {
            this.displayProps = !this.displayProps;
        }
        if (displayProps)
        {
            foreach (System.Reflection.PropertyInfo info in propertyInfo)
            {
                GUILayout.BeginHorizontal();

                GUILayout.Label(info.Name);

                if (!info.CanWrite) {
                    GUILayout.Label(info.PropertyType.ToString());
                    GUILayout.EndHorizontal();
                    continue;
                } 

                object value = info.GetValue(this.co, null);

                switch (info.PropertyType.ToString())
                {
                    case "System.Boolean":
                        info.SetValue(this.co, FieldView.displayBool((bool)value), null);
                        break;
                    case "UnityEngine.Vector3":
                        info.SetValue(this.co, FieldView.displayVector3((Vector3)value), null);
                        break;
                    case "System.Int32":
                        info.SetValue(this.co, FieldView.displayInt((int)value), null);
                        break;
                    case "System.Single":
                        info.SetValue(this.co, FieldView.displayFloat((float)value), null);
                        break;
                    case "System.Double":
                        info.SetValue(this.co, FieldView.displayDouble((double)value), null);
                        break;
                    case "System.String":
                        info.SetValue(this.co, FieldView.displayString((string)value), null);
                        break;
                    case "System.Collections.Generic.List`1[System.String]":
                        info.SetValue(this.co, FieldView.displayListString((List<string>)value), null);
                        break;
                    default:
                        if (value == null)
                        {
                            GUILayout.Label("null");
                        }
                        else
                        {
                            GUILayout.Label(value.ToString());
                        }
                        break;
                }

                GUILayout.EndHorizontal();
            }
        }

        if (GUILayout.Button("Methods", GUILayout.Height(16)))
        {
            this.displayMethod = !this.displayMethod;
        }
        if (displayMethod)
        {
            GUILayout.TextArea(this.methods);
        }
    }
}