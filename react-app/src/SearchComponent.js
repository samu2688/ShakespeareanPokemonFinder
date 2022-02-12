import React, { useState, useRef } from 'react'
import Button from 'react-bootstrap/Button';
import Card from 'react-bootstrap/Card';
import Collapse from 'react-bootstrap/Collapse';
import axios from 'axios';
import './SearchComponent.css';
import configData from "./configuration.json";

export default function SearchComponent() {
  const [open, setOpen] = useState(false);
  const [translation, setTranslation] = useState(null);
  const inputRef = useRef()
  // this.state = {
  //   pokemonTranslation: null
  // }

  function handleOpen(e) {
    const pokemonName = inputRef.current.value
    axios.get(configData.SERVER_URL + '/ShakespeareanPokemon?pokemonName=' + pokemonName)
      // .catch(
      //   res => {
      //     const persons = res.data;
      //     // const ObjFetch = JSON.parse(res.data);
      //     setOpen(!open)
      //   }
      // )
      .then(res => {
        if(!res.data.hasError)
        {
          setTranslation(res.data.translation)
          setOpen(true)
        }
        else
        {
          setTranslation(null)
          setOpen(false)
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
          <div id="example-collapse-text">
            <br></br>
            <div>
              <Card className="search-card" border='secondary'>
                "{translation}"
              </Card>
            </div>
          </div>
        </Collapse>
      </div>
    </>
  );

}
