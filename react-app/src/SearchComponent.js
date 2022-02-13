import React, { useState, useRef } from 'react'
import Button from 'react-bootstrap/Button';
import Card from 'react-bootstrap/Card';
import Collapse from 'react-bootstrap/Collapse';
import Alert from 'react-bootstrap/Alert';
import axios from 'axios';
import './SearchComponent.css';
import configData from "./configuration.json";

export default function SearchComponent() {
  const [open, setOpen] = useState(false);
  const [openAlert, setOpenAlert] = useState(false);
  const [translation, setTranslation] = useState(null);
  const inputRef = useRef()
  // this.state = {
  //   pokemonTranslation: null
  // }

  function handleOpen(e) {
    const pokemonName = inputRef.current.value
    axios.get(configData.SERVER_URL + '/pokemon?pokemon=' + pokemonName)
      .catch(
        res => {
          setTranslation(null)
          setOpen(false)
          setOpenAlert(true)
        }
      )
      .then(res => {
        if(res && !res.data.hasError)
        {
          setTranslation(res.data.translation)
          setOpen(true)
          setOpenAlert(false)
        }
        else
        {
          setTranslation(null)
          setOpen(false)
          setOpenAlert(true)
        }
      })    
  }

  return (
    <>
      <div className='search-input'>
         <input ref={inputRef} type="text" className="form-control" maxLength={20} placeholder="Input Pokemon name" />
         <Button
          onClick={handleOpen}
          aria-controls="example-collapse-text"
          aria-expanded={open}
        >
          Shakespearean traslation
        </Button>
      </div>
      <div>
        <Collapse in={open} dimension="width">
          <div>
            <br></br>
            <div>
              <Card className="search-card" border='secondary'>
                "{translation}"
              </Card>
            </div>
          </div>
        </Collapse>
        <Collapse in={openAlert} dimension="width">
          <div>
            <br></br>
            <div>
              <Alert variant="danger">
                Ops! Something went wrong!
              </Alert>
            </div>
          </div>
        </Collapse>
      </div>
    </>
  );

}
